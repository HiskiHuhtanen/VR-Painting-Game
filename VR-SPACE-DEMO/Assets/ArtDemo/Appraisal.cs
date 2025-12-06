using UnityEngine;

public static class Appraisal
{
    public static float Appraise(Texture2D tex)
    {
        int w = tex.width;
        int h = tex.height;
        int total = w * h;

        int paintedCount = 0;
        float energyAccum = 0f;

        // Use 16 color buckets
        int[] buckets = new int[16];
        Color prev = Color.white;
        for (int x = 0; x < w; x++)
        {
            for (int y = 0; y < h; y++)
            {
                Color c = tex.GetPixel(x, y);

                bool isPainted = c != Color.white;

                if (isPainted)
                {
                    paintedCount++;

                    // Energy score (difference from previous)
                    energyAccum += ColorDifference(prev, c);

                    // Bucket index (for color variety)
                    int bucket = ColorBucket(c);
                    buckets[bucket]++;
                }

                prev = c;
            }
        }

        // ---- Category Scores ----

        float paintFill = (float)paintedCount / total;
        float colorScore = Mathf.Pow(paintFill, 1.5f) * 25000f;  // Increased from 1000f
        int uniqueColorBuckets = CountNonZeroBuckets(buckets);
        float varietyScore = uniqueColorBuckets * uniqueColorBuckets * 2500f;  // Increased from 50f

        float avgEnergy = energyAccum / Mathf.Max(1, paintedCount);
        float energyScore = avgEnergy * 75000f;  // Increased from 2000f

        float rarityMultiplier = RollRarityMultiplier();

        float totalValue = (colorScore + varietyScore + energyScore) * rarityMultiplier;

        return Mathf.Round(totalValue);
    }

    static float ColorDifference(Color a, Color b)
    {
        return Mathf.Abs(a.r - b.r) +
               Mathf.Abs(a.g - b.g) +
               Mathf.Abs(a.b - b.b);
    }

    static int ColorBucket(Color c)
    {
        // Reduce color to 16 buckets
        int r = Mathf.FloorToInt(c.r * 2);
        int g = Mathf.FloorToInt(c.g * 2);
        int b = Mathf.FloorToInt(c.b * 2);
        
        // Clamp values to ensure they're in range [0, 1]
        r = Mathf.Clamp(r, 0, 1);
        g = Mathf.Clamp(g, 0, 1);
        b = Mathf.Clamp(b, 0, 1);
        
        return r + g * 2 + b * 4;
    }

    static int CountNonZeroBuckets(int[] buckets)
    {
        int count = 0;
        foreach (int b in buckets)
            if (b > 0) count++;
        return count;
    }

    static float RollRarityMultiplier()
    {
        float roll = Random.value;
        if (roll > 0.995f) return 50f;    // Increased from 20f - Ultra rare masterpieces
        if (roll > 0.97f) return 15f;     // Increased from 5f - Rare pieces
        if (roll > 0.90f) return 6f;      // Increased from 2.5f - Uncommon art
        if (roll > 0.70f) return 3f;      // Increased from 1.5f - Decent pieces
        return 1.5f;                      // Increased from 1f - Base multiplier
    }
}
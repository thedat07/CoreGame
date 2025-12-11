using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace YNL.Utilities.Extensions
{
    public class GachaItem
    {
        public int index;
        public int dropRate;

        public GachaItem(int index, int dropRate)
        {
            this.index = index;
            this.dropRate = dropRate;
        }
    }

    public class Random<T>
    {
        public static T GetRandomElement(HashSet<T> hashSet)
        {
            int randomIndex = UnityEngine.Random.Range(0, hashSet.Count);
            int currentIndex = 0;
            foreach (T element in hashSet)
            {
                if (currentIndex == randomIndex)
                {
                    return element;
                }
                currentIndex++;
            }
            throw new System.Exception("HashSet is empty or out of bounds");
        }

        public static T GetRandomElement(List<T> list)
        {
            int randomIndex = UnityEngine.Random.Range(0, list.Count);
            return list[randomIndex];
        }

        public static List<T> GetRandom(List<T> list)
        {
            System.Random random = new System.Random();
            return list.OrderBy(x => random.Next()).ToList();
        }

        public static int RollGacha(List<GachaItem> data)
        {
            // Total drop rate
            float totalDropRate = 0f;

            // Calculate the total drop rate
            foreach (var item in data)
            {
                totalDropRate += item.dropRate;
            }

            // Generate a random number between 0 and the total drop rate
            float randomValue = UnityEngine.Random.Range(0f, totalDropRate);

            // Loop through the items in the gacha
            foreach (var item in data)
            {
                // Subtract the drop rate of the current item from the random value
                randomValue -= item.dropRate;

                // If the random value is less than or equal to 0, return the item
                if (randomValue <= 0)
                {
                    return item.index;
                }
            }

            // Return null if no item is obtained
            return -1;
        }
    }
}
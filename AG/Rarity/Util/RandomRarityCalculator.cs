using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using AG.Base.Util;
using AG.Base.Variable;
using AG.RarityMechanic.Enum;

namespace AG.RarityMechanic.Util
{
    public class RandomRarityCalculator : LuckyRandomGenerator
    {
        private ReadOnlyDictionary<Rarity, float> _rarityChances;

        public RandomRarityCalculator(FloatVariable luck) : base(luck)
        {
            //Weighted Rarities
            _rarityChances = new ReadOnlyDictionary<Rarity, float>
            (
                new Dictionary<Rarity, float>
                {
                    { Rarity.Common, 45f },
                    { Rarity.Uncommon, 35f },
                    { Rarity.Rare, 14f },
                    { Rarity.Epic, 4f },
                    { Rarity.Legendary, 2f },
                }
            );
        }

        //Set Min Rarity If Valid Rarity Desired
        public Rarity GetRandomRarity(Rarity maxRarity, Rarity minRarity = Rarity.Common)
        {
            Dictionary<Rarity, float> tempRarities = new(_rarityChances);

            //Increase Chances Of Some Rarities
            tempRarities[Rarity.Rare] *= luck.Value;
            tempRarities[Rarity.Epic] *= luck.Value;
            tempRarities[Rarity.Legendary] *= luck.Value;

            float rarityVal = GetRandomValue((int)tempRarities.Values.Sum());

            foreach (Rarity rarity in tempRarities.Keys)
            {
                rarityVal -= tempRarities[rarity];

                if (rarityVal < 0)
                {
                    //Rarity Selected And Checking For If It Is Higher Than Max Rarity
                    if (rarity <= maxRarity)
                    {
                        if (minRarity <= rarity)
                        {
                            return rarity;
                        }
                        //If Min Rarity Is Higher Than Selected Rarity Return Min Rarity
                        return minRarity;
                    }
                    //It Is Higher Than Max Rarity, Break So We Can Return Max Rarity For Valid Rarity
                    break;
                }
            }
            return maxRarity;
        }
    }
}
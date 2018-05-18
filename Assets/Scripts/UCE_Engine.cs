namespace UCE
{
    using UnityEngine;
    using System.Collections.Generic;
    public class UCE_Engine
    {
        public enum ChemicalType : int
        {
            HCL, CU, CUCL2, H2O, H2O2, O2,
        }

        [System.Serializable]
        public class ChemicalAmount
        {
            public ChemicalType type;
            public int amount = 1;

            public ChemicalAmount(ChemicalType _type, int _amount)
            {
                type = _type;
                amount = _amount;
            }
        }

        public class ReactionResult
        {
            UCE_Animation.Animation animation;
            List<ChemicalAmount> chemical;

            public ReactionResult(UCE_Animation.Animation _animation, List<ChemicalAmount> _chemical)
            {
                animation = _animation;
                chemical = _chemical;
            }
        }

        public static int env_temperature;

        public static int env_pressure;

        // This array stores the animation type when two chemical react
        // It should be changed when we need to have different reaction in different environment
        public static int[,] reactionTable = new int[2, 2]
        {
            {0, 3 },
            {1, 0 },
        };

        // TODO: 这个返回ReactionResult，储存反应后各种物质的剩余量会更好，但是还没想到怎么优雅的和其他模块交流:(
        public static UCE_Animation.Animation React(List<ChemicalAmount> reactants)
        {
            if (reactants.Count < 2)
            {
                Debug.LogWarning("Insufficient reactants");
                return UCE_Animation.Animation.NoAnimation;
            }
            return (UCE_Animation.Animation)reactionTable[(int)reactants[0].type, (int)reactants[1].type];
        }
    }
}

using Unity.Mathematics;

namespace Noises.PointBasedNoises
{
    public class WorleyNoiseTerrain : NoiseTerrain
    {
        public float heightMultiplier = 20f;
        public float heightAddition = 10f;
        public float scale = 0.025f;
        protected override float GenerateNoise(float x, float z)
        {
            var coord = new float2(x * scale, z * scale);
            
            return noise.cellular(coord).x * heightMultiplier - heightAddition;
        }
    }
}
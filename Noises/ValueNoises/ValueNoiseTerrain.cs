using Unity.Mathematics;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace Noises.ValueNoises
{
    public class ValueNoiseTerrain : NoiseTerrain
    {
        private const int KMaxTableSize = 256;
        private const int KMaxTableSizeMask = KMaxTableSize - 1;
        private readonly float[] _r = new float[KMaxTableSize * KMaxTableSize];
        
        public float heightMultiplier = 20f;
        public float heightAddition = 10f;
        public float scale = 0.025f;
        public uint seed = 2016;
        
        protected override float GenerateNoise(float x, float z)
        {
            var coord = new float2(x * scale, z * scale);
            
            return Eval(coord) * heightMultiplier - heightAddition;
        }
        
        ValueNoiseTerrain()
        {
            Debug.Log("ValueNoiseTerrain");
            var rand = new Random(seed);
            for (var k = 0; k < _r.Length; ++k)
            {
                _r[k] = rand.NextFloat();
            }
        }
        
        public float Eval(float2 p)
        {
            var xi = (int)math.floor(p.x);
            var yi = (int)math.floor(p.y);
            
            float tx = p.x - xi, ty = p.y - yi;
            
            int rx0 = xi & KMaxTableSizeMask, rx1 = (rx0 + 1) & KMaxTableSizeMask;
            int ry0 = yi & KMaxTableSizeMask, ry1 = (ry0 + 1) & KMaxTableSizeMask;

            var c00 = _r[ry0 * KMaxTableSize + rx0];
            var c10 = _r[ry0 * KMaxTableSize + rx1];
            var c01 = _r[ry1 * KMaxTableSize + rx0];
            var c11 = _r[ry1 * KMaxTableSize + rx1];

            float sx = Smoothstep(tx), sy = Smoothstep(ty);
            float nx0 = Lerp(c00, c10, sx), nx1 = Lerp(c01, c11, sx);
            
            return Lerp(nx0, nx1, sy);
        }

        private static float Lerp(float lo, float hi, float t)
        {
            return math.lerp(lo, hi, t);
        }

        private static float Smoothstep(float t)
        {
            return t * t * (3 - 2 * t);
        }
    }
}
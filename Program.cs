using System.Numerics;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace MyBenchmarks
{
    public class MicrobeSearch 
    {
        [Params(0.05f,0.2f,0.5f)]
        public float SPECIES_FRACTION { get; set; }

        [Params(50,150,350)]
        public float SEARCH_RADIUS { get; set; }

        [Params(35)]
        public int MICROBE_AMOUNT { get; set; }

        private readonly Vector3 POSITION = new Vector3(0, 0, 0);

        private const float SPAWN_RANGE = 350;

        Random RND = new Random();


        [Benchmark]
        public void FindSpeciesNearPointBranch()
        {
            float nearestDistanceSquared = float.MaxValue;
            float searchRadiusSquared = SEARCH_RADIUS * SEARCH_RADIUS;

            foreach (int val in Enumerable.Range(1,MICROBE_AMOUNT))
            {
                Vector3 microbeposition = new Vector3((float) RND.NextDouble()*SPAWN_RANGE,
                    (float) RND.NextDouble()*SPAWN_RANGE, (float) RND.NextDouble()*SPAWN_RANGE);

                if ((RND.NextDouble()+1)/2>SPECIES_FRACTION)
                    continue;

                // Skip candidates for performance
                if (Math.Abs(microbeposition.X - POSITION.X) > SEARCH_RADIUS ||
                    Math.Abs(microbeposition.Y - POSITION.Y) > SEARCH_RADIUS)
                {
                    continue;
                }

                var distanceSquared = (microbeposition - POSITION).LengthSquared();

                if (distanceSquared < nearestDistanceSquared &&
                    distanceSquared < searchRadiusSquared &&
                    distanceSquared > 1)
                {
                    nearestDistanceSquared = distanceSquared;
                }
            }
        }

        [Benchmark]
        public void FindSpeciesNearPointNoBranch()
        {
            float nearestDistanceSquared = float.MaxValue;
            float searchRadiusSquared = SEARCH_RADIUS * SEARCH_RADIUS;

            foreach (int val in Enumerable.Range(1,MICROBE_AMOUNT))
            {
                Vector3 microbeposition = new Vector3((float) RND.NextDouble()*SPAWN_RANGE,
                    (float) RND.NextDouble()*SPAWN_RANGE, (float) RND.NextDouble()*SPAWN_RANGE);

                if ((RND.NextDouble()+1)/2>SPECIES_FRACTION)
                    continue;

                var distanceSquared = (microbeposition - POSITION).LengthSquared();

                if (distanceSquared < nearestDistanceSquared &&
                    distanceSquared < searchRadiusSquared &&
                    distanceSquared > 1)
                {
                    nearestDistanceSquared = distanceSquared;
                }
            }
        }
    }
    public class Program
    {
        public static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<MicrobeSearch>();
        }
    }
}
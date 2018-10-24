
namespace SpaceBattle
{
    [System.Serializable]
    public struct Boundary
    {
        public float minX;
        public float maxX;
        
        public float minY;
        public float maxY;

        public Boundary(Boundary b)
        {
            minX = b.minX;
            maxX = b.maxX;
            minY = b.minY;
            maxY = b.maxY;
        }

        public Boundary(Boundary b, float koef)
        {
            minX = b.minX * koef;
            maxX = b.maxX * koef;
            minY = b.minY * koef;
            maxY = b.maxY * koef;
        }

        public void Scale (float scale)
        {
            minX *= scale;
            maxX *= scale;
            minY *= scale;
            maxY *= scale;
        }
    }
}

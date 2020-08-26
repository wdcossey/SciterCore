﻿namespace SciterCore
{
    public readonly struct PolylinePoint
    {
        public float X { get; }
        
        public float Y { get; }
        
        private PolylinePoint(float x, float y)
        {
            X = x;
            Y = y;
        }
        
        public static PolylinePoint Create(float x, float y)
        {
            return new PolylinePoint(x: x, y: y);
        }
    }
}
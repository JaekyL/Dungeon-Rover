using System;
using Unity.Collections;

namespace Helper
{
    public static class ArrayHelper
    {
        public static void Set2D<T>(this NativeArray<T> array, int x, int y, T value, int width) where T : unmanaged
        {
            int flatIndex = FlattenIndex(x, y, width);
            
            if (flatIndex < 0 || flatIndex >= array.Length)
            {
                throw new IndexOutOfRangeException($"Index {flatIndex} is out of range {array.Length}. [{x},{y}], width:{width}");
            }
            
            array[flatIndex] = value;
        }
 
        
        public static T Get2D<T>(this NativeArray<T> array, int x, int y, int width) where T : unmanaged
        {
            int flatIndex = FlattenIndex(x, y, width);

            if (flatIndex < 0 || flatIndex >= array.Length)
            {
                throw new IndexOutOfRangeException($"Index {flatIndex} is out of range {array.Length}. ({x},{y}), width:{width}");
            }
            
            return array[flatIndex];
        }
 
        public static int FlattenIndex(int x, int y, int width) => y * width + x;
    }
}
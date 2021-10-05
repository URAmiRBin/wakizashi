namespace UbiRock.Wakizashi.Toolkit {
    public static class ArrayHelper {
        public static T[] SliceTo<T>(T[] array, int length) {
            T[] slicedArray = new T[length];
            for(int i = 0; i < length; i++) {
                slicedArray[i] = array[i];
            }
            return slicedArray;
        }
    }
}
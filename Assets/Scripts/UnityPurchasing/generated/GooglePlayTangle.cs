// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("mDAA9ruXqA6VYDg6+n3g/3iov79pMY/kywNghBKaNyFgg1x0+n+ihDoIwMokA+5jYUurdC7TWJwRxoFRmlkeZLrkYGJV1mJeox+CT1EFGGoCIC1exMywakjWML780KL5KWR7p8DDUghISfZi9zyE07Nq9vTPx85gAzjB3Nz3uy8AEe+A2QcF2pxhSvEcihTHFlcaWQOcLyuCp2lPwmV82jTfWT3M7lOxYI/6HSjGYOrUmXIVQPJxUkB9dnla9jj2h31xcXF1cHOQFO8FTLg9JyKw7ViYOQ76gpmoZvJxf3BA8nF6cvJxcXDykuyi7uQGXeu+qywK1RJQGN+fobKn0Q+D0F5Wl9l1u9DkZoCGw2OA/D2I3X40uxrPuUZfYv3ccXJzcXBx");
        private static int[] order = new int[] { 9,6,7,13,10,11,10,7,13,9,11,11,12,13,14 };
        private static int key = 112;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}

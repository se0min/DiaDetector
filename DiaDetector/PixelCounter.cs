using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ImageUtilmode;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections;

namespace DiaDetector
{
    public class PixelCounter
    {
        public static MainFrame fm;
        public static int CountPixelbyGetPixel(Bitmap image, Color selectedColor)   //GetPixel을 이용한 코드
        {
            int PixelCount = 0;

            for (int Y = 0; Y < image.Height; Y++)
            {
                for (int X = 0; X < image.Width; X++)
                {
                    // GetPixel이라는 함수를 통해 Pixel의 값을 읽어옴
                    Color ImageColor = image.GetPixel(X, Y);
                    if (IsSameColor(ImageColor,selectedColor) == true)
                    {
                        PixelCount++;
                    }
                }
            }

            return PixelCount;
        }
        public static int Xacel=90;
        public static List<int> HongLine = new List<int>();
        public static List<int> HongPoint1 = new List<int>();
        public static List<int> HongPoint2 = new List<int>();
        public static List<int> HongPoint3 = new List<int>();
        public static List<int> HongPoint4 = new List<int>();
        public static List<int> HongPoint5 = new List<int>();
        public static List<int> HongPoint6 = new List<int>();
        public static List<int> HongPoint7 = new List<int>();
        public static List<int> HongPoint8 = new List<int>();

        public static List<int> HongLine2 = new List<int>();
        public static List<int> HongPoint9 = new List<int>();
        public static List<int> HongPoint10 = new List<int>();
        public static List<int> HongPoint11 = new List<int>();
        public static List<int> HongPoint12 = new List<int>();
        public static List<int> HongPoint13 = new List<int>();
        public static List<int> HongPoint14 = new List<int>();
        public static List<int> HongPoint15 = new List<int>();
        public static List<int> HongPoint16 = new List<int>();
       public static int CountPixelbyPointer(Bitmap image) //Pointer를 이용한 코드
        {
            int ImageIndex = 0;
            int PixelCount = 0;
            RGB ImageRGB;
            HongLine.Clear();
            HongPoint1.Clear();
            HongPoint2.Clear();
            HongPoint3.Clear();
            HongPoint4.Clear();
            HongPoint5.Clear();
            HongPoint6.Clear();
            HongPoint7.Clear();
            HongPoint8.Clear();
            unsafe
            {
                // unsafe한 Pointer를 이용하여 각각의 값을 가져옴
                RGB* RGBImage = (RGB*)image.LockMemory(ImageLockMode.ReadOnly).ToPointer();
                for (int X = 250; X <= 950; X += 100)
                {
                    for (int Y = 100; Y < image.Height - 100; Y++)
                    {
                        ImageIndex = Y * image.Width + X;
                        ImageRGB = RGBImage[ImageIndex];

                        // if (IsSameColor(selectedColor,ImageRGB) == true)
                        if (ImageRGB.B >= Xacel && X == 250)
                        {
                            PixelCount++;
                            HongPoint1.Add(Y);
                        }
                        if (ImageRGB.B >= Xacel && X == 350)
                        {
                            PixelCount++;
                            HongPoint2.Add(Y);
                        }
                        if (ImageRGB.B >= Xacel && X == 450)
                        {
                            PixelCount++;
                            HongPoint3.Add(Y);
                        }
                        if (ImageRGB.B >= Xacel && X == 550)
                        {
                            PixelCount++;
                            HongPoint4.Add(Y);
                        }
                        if (ImageRGB.B >= Xacel && X == 650)
                        {
                            PixelCount++;
                            HongPoint5.Add(Y);
                        }
                        if (ImageRGB.B >= Xacel && X == 750)
                        {
                            PixelCount++;
                            HongPoint6.Add(Y);
                        }
                        if (ImageRGB.B >= Xacel && X == 850)
                        {
                            PixelCount++;
                            HongPoint7.Add(Y);
                        }
                        if (ImageRGB.B >= Xacel && X == 950)
                        {
                            PixelCount++;
                            HongPoint8.Add(Y);
                        }
                        
                    }
                }
                
            }
            image.UnLockMemory();
            image.Dispose();    
            return PixelCount;
        }
       public static int CountPixelbyPointer2(Bitmap image) //Pointer를 이용한 코드
       {
           int ImageIndex = 0;
           int PixelCount = 0;
           RGB2 ImageRGB;
           HongLine2.Clear();
           HongPoint9.Clear();
           HongPoint10.Clear();
           HongPoint11.Clear();
           HongPoint12.Clear();
           HongPoint13.Clear();
           HongPoint14.Clear();
           HongPoint15.Clear();
           HongPoint16.Clear();

           unsafe
           {
               // unsafe한 Pointer를 이용하여 각각의 값을 가져옴
               RGB2* RGBImage = (RGB2*)image.LockMemory(ImageLockMode.ReadOnly).ToPointer();

               for (int X = 250; X <= 950; X += 100)
               {
                 for (int Y = 100; Y < image.Height - 100; Y++)

                   {
                       ImageIndex = Y * image.Width + X;
                       ImageRGB = RGBImage[ImageIndex];

                       // if (IsSameColor(selectedColor,ImageRGB) == true)
                       if (ImageRGB.B >= Xacel && X == 250)
                       {
                           PixelCount++;
                           HongPoint9.Add(Y);
                       }
                       if (ImageRGB.B >= Xacel && X == 350)
                       {
                           PixelCount++;
                           HongPoint10.Add(Y);
                       }
                       if (ImageRGB.B >= Xacel && X == 450)
                       {
                           PixelCount++;
                           HongPoint11.Add(Y);
                       }
                       if (ImageRGB.B >= Xacel && X == 550)
                       {
                           PixelCount++;
                           HongPoint12.Add(Y);
                       }
                       if (ImageRGB.B >= Xacel && X == 650)
                       {
                           PixelCount++;
                           HongPoint13.Add(Y);
                       }
                       if (ImageRGB.B >= Xacel && X == 750)
                       {
                           PixelCount++;
                           HongPoint14.Add(Y);
                       }
                       if (ImageRGB.B >= Xacel && X == 850)
                       {
                           PixelCount++;
                           HongPoint15.Add(Y);
                       }
                       if (ImageRGB.B >= Xacel && X == 950)
                       {
                           PixelCount++;
                           HongPoint16.Add(Y);
                       }
                      
                   }

               }
               image.UnLockMemory2();
               image.Dispose();
               return PixelCount;
           }
         
       
       }
        private static bool IsSameColor(Color A, Color B)
        {
            if (A.R == B.R && A.G == B.G && A.B == B.B) 
                return true;
            else 
                return false;
        }

        private static bool IsSameColor(Color A, RGB B)
        {
            if (A.R == B.R && A.G == B.G && A.B == B.B)
                return true;
            else
                return false;
        }
    }
}

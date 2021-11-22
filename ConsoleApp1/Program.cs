using System;
using System.Diagnostics;
using MathNet.Numerics.LinearAlgebra.Double;
using System.Collections;
using System.Collections.Generic;

using MathNet.Numerics.IntegralTransforms;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            FFT a=new FFT();
            float[] b =a.ResultArr();
            Console.WriteLine(b[1]);


            ////////////////////////////////////////////////
            //测试是弧度制还是角度制
            //Console.WriteLine(Math.Cos(60 * Math.PI / 180));
            //////////////////////////////////////////


            //double[] velocty = new double[2];
            //double[] Acceleration_y = new double[2];
            //double time = 0.02;
            //double[] distance = new double[2];
            //distance[0] = 0;
            //velocty[0] = 0;
            //Acceleration_y[0] = 0;

            //for (int i=0;i<147;i++)
            //{
            //    Acceleration_y[1] = i;
            //    distance[0] = distance[0] + time * velocty[0] + 1 / (6 * time) * (Acceleration_y[1] - Acceleration_y[0]) * Math.Pow(time, 3) + 0.5 * Acceleration_y[0] * Math.Pow(time, 2);
            //    velocty[0] = velocty[0] + (Acceleration_y[1] + Acceleration_y[0]) * time / 2;
            //    Acceleration_y[0] = Acceleration_y[1];
            //    //distance[0] = distance[0] + time * velocty[0]+ 0.5 * Acceleration_y[1] * Math.Pow(time, 2);
            //    //velocty[0] = velocty[0] + Acceleration_y[1]* time ;
            //    //Acceleration_y[0] = Acceleration_y[1];
            //    Console.WriteLine("Acceleration_y:{0}", Acceleration_y[1]);
            //    Console.WriteLine(distance[0]);
            //}



            //Acceleration_y[0] = 0.025;
            //Acceleration_y[1] = 0.05;
            //distance[0] = distance[0] + time * velocty[0] + 1 / (6 * time) * (Acceleration_y[1] - Acceleration_y[0]) * Math.Pow(time, 3) + 0.5 * Acceleration_y[0] * Math.Pow(time, 2);
            //velocty[0] = velocty[0] + (Acceleration_y[1] + Acceleration_y[0]) * time / 2;

            //Console.WriteLine(distance[0]);

            //Acceleration_y[0] = 0.05;
            //Acceleration_y[1] = 0.075;
            //distance[0] = distance[0] + time * velocty[0] + 1 / (6 * time) * (Acceleration_y[1] - Acceleration_y[0]) * Math.Pow(time, 3) + 0.5 * Acceleration_y[0] * Math.Pow(time, 2);
            //velocty[0] = velocty[0] + (Acceleration_y[1] + Acceleration_y[0]) * time / 2;

            //Console.WriteLine(distance[0]);


            //通过求重力加速度在x轴上的分力，抵消静止时加速的
            //DenseMatrix A = new DenseMatrix(3, 3);
            //DenseMatrix A_2 = new DenseMatrix(3, 3);
            //DenseMatrix B = new DenseMatrix(3, 1);
            //DenseMatrix C_2 = new DenseMatrix(3, 3);
            //double a;

            //double Az = -16.4*Math.PI/180;
            //double Ax = -0.24* Math.PI / 180;
            //double Ay = 0.33* Math.PI / 180;
            //A[0, 0] = Math.Cos(Az) * Math.Cos(Ay);
            //A[0, 1] = Math.Cos(Az) * Math.Sin(Ay) * Math.Sin(Ax) - Math.Sin(Az) * Math.Cos(Ax);
            //A[0, 2] = Math.Cos(Az) * Math.Sin(Ay) * Math.Cos(Ax) + Math.Sin(Az) * Math.Sin(Ax);
            //A[1, 0] = Math.Sin(Az) * Math.Cos(Ay);
            //A[1, 1] = Math.Sin(Az) * Math.Sin(Ay) * Math.Sin(Ax) + Math.Cos(Az) * Math.Cos(Ax);
            //A[1, 2] = Math.Sin(Az) * Math.Sin(Ay) * Math.Cos(Ax) - Math.Cos(Az) * Math.Sin(Ax);
            //A[2, 0] = -Math.Sin(Ay);
            //A[2, 1] = Math.Cos(Ay) * Math.Sin(Ax);
            //A[2, 2] = Math.Cos(Ay) * Math.Cos(Ax);
            //B[0, 0] = 0;
            //B[1, 0] = 0;
            //B[2, 0] = 1;
            //B = (DenseMatrix)(A.Transpose() * B);
            //Console.WriteLine(A);
            //Console.WriteLine(B);
            //B = B * 9.8;
            //Console.WriteLine(B);


            /////////////////////////////////////////////////////////////////////////////////
            //DenseMatrix A = new DenseMatrix(3, 3);
            //DenseMatrix A_2 = new DenseMatrix(3, 3);
            //DenseMatrix B = new DenseMatrix(3, 1);
            //DenseMatrix C_2 = new DenseMatrix(3, 3);

            //double Az =0;
            //double Ax = 0;
            //double Ay = Math.PI / 6;
            //Console.WriteLine("Hello World!");
            //Math.Cos(Ay);
            //A[0, 0] = Math.Cos(Az) * Math.Cos(Ay);
            //A[0, 1] = Math.Cos(Az) * Math.Sin(Ay) * Math.Sin(Ax) - Math.Sin(Az) * Math.Cos(Ax);
            //A[0, 2] = Math.Cos(Az) * Math.Sin(Ay) * Math.Cos(Ax) + Math.Sin(Az) * Math.Sin(Ax);
            //A[1, 0] = Math.Sin(Az) * Math.Cos(Ay);
            //A[1, 1] = Math.Sin(Az) * Math.Sin(Ay) * Math.Sin(Ax) + Math.Cos(Az) * Math.Cos(Ax);
            //A[1, 2] = Math.Sin(Az) * Math.Sin(Ay) * Math.Cos(Ax) - Math.Cos(Az) * Math.Sin(Ax);
            //A[2, 0] = -Math.Sin(Ay);
            //A[2, 1] = Math.Cos(Ay) * Math.Sin(Ax);
            //A[2, 2] = Math.Cos(Ay) * Math.Cos(Ax);
            //Ay = Math.PI / 3;

            //A_2[0, 0] = Math.Cos(Az) * Math.Cos(Ay );
            //A_2[0, 1] = Math.Cos(Az ) * Math.Sin(Ay ) * Math.Sin(Ax) - Math.Sin(Az ) * Math.Cos(Ax );
            //A_2[0, 2] = Math.Cos(Az ) * Math.Sin(Ay) * Math.Cos(Ax) + Math.Sin(Az) * Math.Sin(Ax );
            //A_2[1, 0] = Math.Sin(Az ) * Math.Cos(Ay);
            //A_2[1, 1] = Math.Sin(Az ) * Math.Sin(Ay ) * Math.Sin(Ax) + Math.Cos(Az ) * Math.Cos(Ax);
            //A_2[1, 2] = Math.Sin(Az) * Math.Sin(Ay ) * Math.Cos(Ax ) - Math.Cos(Az) * Math.Sin(Ax );
            //A_2[2, 0] = -Math.Sin(Ay );
            //A_2[2, 1] = Math.Cos(Ay ) * Math.Sin(Ax );
            //A_2[2, 2] = Math.Cos(Ay ) * Math.Cos(Ax );

            //C_2 = (DenseMatrix)(A.Transpose() * A_2);
            //C_2 = (DenseMatrix)(C_2.Transpose());
            ////C = (DenseMatrix)(A.Transpose() * B);

            //B[1, 0] = Math.Atan2(-C_2[2, 0], Math.Sqrt(Math.Pow(C_2[0, 0], 2) + Math.Pow(C_2[1, 0], 2))) * (180 / Math.PI);
            //B[2, 0] = Math.Atan2(C_2[1, 0] / Math.Cos(B[1, 0] * (Math.PI / 180)), C_2[0, 0] / Math.Cos(B[1, 0] * (Math.PI / 180))) * (180 / Math.PI);
            //B[0, 0] = Math.Atan2(C_2[2, 1] / Math.Cos(B[1, 0] * (Math.PI / 180)), C_2[2, 2] / Math.Cos(B[1, 0] * (Math.PI / 180))) * (180 / Math.PI);
            //Console.WriteLine(A);
            //Console.WriteLine(A_2);
            //Console.WriteLine(B);

        }

    }


    public class FFT
    {
        MathNet.Numerics.Complex32[] mathNetComplexArrRe = new MathNet.Numerics.Complex32[64];
        float[] data = new float[] { 0, 3, 2, 5, 3, -7, -6, -9, -5, -13, -12, -15, -13, 17, 6, 19, 10, 13, 22, 22, 3, 27, 36, 19, 25, 13, 52, 45, 33, 22, 6, 19, 0, 3, 2, 5, 3, -7, -6, -9, -5, -13, -12, -15, -13, 17, 6, 19, 10, 13, 22, 22, 3, 27, 36, 19, 25, 13, 52, 45, 33, 22, 6, 19 };
        float[] resultArr = new float[64];
        public float[] ResultArr()
        {
            float[] filterArr = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
            resultArr = Filter(data, filterArr);
            return resultArr;

        }
        /// <summary>
        /// 滤波数组
        /// </summary>
        /// <param name="inData">输入的数据</param>
        /// <param name="filterArr">滤波数组，可以自定义</param>
        /// <returns></returns>
        public float[] Filter(float[] inData, float[] filterArr)
        {
            float[] outArr = new float[64];
            outArr = inData;
            MathNet.Numerics.Complex32[] mathNetComplexArr = new MathNet.Numerics.Complex32[64];
            for (int i = 0; i < mathNetComplexArr.Length; i++)
            {
                mathNetComplexArr[i] = new MathNet.Numerics.Complex32((float)outArr[i], 0);
            }
            Fourier.Forward(mathNetComplexArr);//傅里叶变换
            for (int i = 0; i < mathNetComplexArr.Length; i++)
            {
                mathNetComplexArr[i] = new MathNet.Numerics.Complex32(mathNetComplexArr[i].Real * filterArr[i], mathNetComplexArr[i].Imaginary * filterArr[i]);
            }
            float[] ArrFreq = new float[64];
            for (int i = 0; i < ArrFreq.Length; i++)
            {
                ArrFreq[i] = (float)Math.Sqrt(mathNetComplexArr[i].Imaginary * mathNetComplexArr[i].Imaginary + mathNetComplexArr[i].Real * mathNetComplexArr[i].Real);//利用LineRenderer显示频域结果
            }
            Fourier.Inverse(mathNetComplexArr);//逆傅里叶变换
            for (int i = 0; i < mathNetComplexArr.Length; i++)
            {
                outArr[i] = mathNetComplexArr[i].Real;
            }
            return outArr;
        }
    }
}

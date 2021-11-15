using System;
using MathNet.Numerics.LinearAlgebra.Double;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {//通过求重力加速度在x轴上的分力，抵消静止时加速的
            DenseMatrix A = new DenseMatrix(3, 3);
            DenseMatrix A_2 = new DenseMatrix(3, 3);
            DenseMatrix B = new DenseMatrix(3, 1);
            DenseMatrix C_2 = new DenseMatrix(3, 3);
            double a;

            double Az = 39.3*Math.PI/180;
            double Ax = -0.59* Math.PI / 180;
            double Ay = -0.15 * Math.PI / 180;
            A[0, 0] = Math.Cos(Az) * Math.Cos(Ay);
            A[0, 1] = Math.Cos(Az) * Math.Sin(Ay) * Math.Sin(Ax) - Math.Sin(Az) * Math.Cos(Ax);
            A[0, 2] = Math.Cos(Az) * Math.Sin(Ay) * Math.Cos(Ax) + Math.Sin(Az) * Math.Sin(Ax);
            A[1, 0] = Math.Sin(Az) * Math.Cos(Ay);
            A[1, 1] = Math.Sin(Az) * Math.Sin(Ay) * Math.Sin(Ax) + Math.Cos(Az) * Math.Cos(Ax);
            A[1, 2] = Math.Sin(Az) * Math.Sin(Ay) * Math.Cos(Ax) - Math.Cos(Az) * Math.Sin(Ax);
            A[2, 0] = -Math.Sin(Ay);
            A[2, 1] = Math.Cos(Ay) * Math.Sin(Ax);
            A[2, 2] = Math.Cos(Ay) * Math.Cos(Ax);
            B[0, 0] = 0;
            B[1, 0] = 0;
            B[2, 0] = 1;
            B = (DenseMatrix)(A.Transpose() * B);
            Console.WriteLine(A);
            Console.WriteLine(B);
            a = Math.Sin(B[0, 0]);
            Console.WriteLine(a);
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
}

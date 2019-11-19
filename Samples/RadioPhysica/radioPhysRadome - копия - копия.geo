// Gmsh project created on Thu Feb 08 13:00:10 2018
SetFactory("OpenCASCADE");
//+
r = DefineNumber[ 9364, Name "Parameters/r" ];
//+
Sphere(1) = {0, 0, 0, r, -Pi/2, Pi/2, 2*Pi};
//+
off = DefineNumber[ 1469, Name "Parameters/off" ];
//+
Box(2) = {-r*2+off, -r, -r, 2*r, 2*r, 2*r};
//+
BooleanDifference{ Volume{1}; Delete; }{ Volume{2}; Delete; }

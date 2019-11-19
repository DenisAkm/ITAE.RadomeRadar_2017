// Gmsh project created on Wed Feb 07 21:22:30 2018
SetFactory("OpenCASCADE");
//+
a = DefineNumber[ 4390, Name "Parameters/a" ];
//+
h = DefineNumber[ a/2/0.4142135623730950488016887242097, Name "Parameters/h" ];
//+
off = DefineNumber[ 1469, Name "Parameters/offset" ];
//+
Point(1) = {off, a/2, h, 1.0};
//+
Point(2) = {off, -a/2, h, 1.0};
//+
Point(3) = {off, a/2, -h, 1.0};
//+
Point(4) = {off, -a/2, -h, 1.0};
//+
Point(5) = {off, h, a/2, 1.0};
//+
Point(6) = {off, -h, a/2, 1.0};
//+
Point(7) = {off, h, -a/2, 1.0};
//+
Point(8) = {off, -h, -a/2, 1.0};
//+
Line(1) = {6, 2};
//+
Line(2) = {2, 1};
//+
Line(3) = {1, 5};
//+
Line(4) = {5, 7};
//+
Line(5) = {7, 3};
//+
Line(6) = {3, 4};
//+
Line(7) = {4, 8};
//+
Line(8) = {8, 6};
//+
Extrude {0, 0, 1} {
  Line{7}; 
}
//+
Line Loop(2) = {3, 4, 5, 6, 7, 8, 1, 2};
//+
Plane Surface(2) = {2};

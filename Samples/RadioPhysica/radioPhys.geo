// Gmsh project created on Wed Feb 07 13:34:40 2018
SetFactory("OpenCASCADE");
//+
a = DefineNumber[ 4390, Name "Parameters/a" ];
//+
h = DefineNumber[ a/2/0.4142135623730950488016887242097, Name "Parameters/h" ];
//+
offset = DefineNumber[ 1469, Name "Parameters/offset" ];
//+
Point(1) = {0, 0, 0, 1.0};
//+
Point(2) = {offset, a/2, h, 1.0};
//+
Point(3) = {offset, -a/2, h, 1.0};
//+
Point(4) = {offset, 0, 0, 1.0};
//+
Recursive Delete {
  Point{1}; 
}
//+
Line(1) = {4, 3};
//+
Line(2) = {2, 3};
//+
Line(3) = {4, 2};
//+
Line Loop(1) = {1, -2, -3};
//+
Surface(1) = {1};
//+
Rotate {{1, 0, 0}, {0, 0, 0}, Pi/4} {
  Duplicata { Surface{1}; }
}
//+
Rotate {{1, 0, 0}, {0, 0, 0}, Pi/4} {
  Duplicata { Surface{2}; }
}
//+
Rotate {{1, 0, 0}, {0, 0, 0}, Pi/4} {
  Duplicata { Surface{3}; }
}
//+
Rotate {{1, 0, 0}, {0, 0, 0}, Pi/4} {
  Duplicata { Surface{4}; }
}
//+
Rotate {{1, 0, 0}, {0, 0, 0}, Pi/4} {
  Duplicata { Surface{4}; }
}
//+
Recursive Delete {
  Surface{4}; 
}
//+
Recursive Delete {
  Surface{5}; 
}
//+
Recursive Delete {
  Surface{6}; 
}
//+
Rotate {{1, 0, 0}, {0, 0, 0}, Pi/4} {
  Duplicata { Surface{3}; }
}
//+
Rotate {{1, 0, 0}, {0, 0, 0}, Pi/4} {
  Duplicata { Surface{4}; }
}
//+
Rotate {{1, 0, 0}, {0, 0, 0}, Pi/4} {
  Duplicata { Surface{5}; }
}
//+
Rotate {{1, 0, 0}, {0, 0, 0}, Pi/4} {
  Duplicata { Surface{6}; }
}
//+
Rotate {{1, 0, 0}, {0, 0, 0}, Pi/4} {
  Duplicata { Surface{7}; }
}
//+
BooleanUnion{ Surface{6}; Surface{7}; Delete; }{ Surface{8}; Delete; }
//+
BooleanUnion{ Surface{6}; Delete; }{ Surface{7}; Delete; }
//+
BooleanUnion{ Surface{10}; Delete; }{ Surface{8}; Delete; }
//+
BooleanUnion{ Surface{11}; Delete; }{ Surface{1}; Delete; }

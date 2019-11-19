// Gmsh project created on Wed Dec 27 12:59:12 2017
SetFactory("OpenCASCADE");
//+
Point(1) = {0, 0, 0, 1.0};
//+
Point(2) = {1, 0, 0, 1.0};
//+
Point(3) = {1.3, 0, 0, 1.0};
//+
Recursive Delete {
  Point{3}; 
}
//+
Point(3) = {2.6, 0.1, 0.5, 1.0};
//+
Recursive Delete {
  Point{3}; 
}
//+
Point(3) = {-1, 0, 0, 1.0};
//+
Sphere(1) = {0, 0, 0, 1, 0, Pi/2, 2*Pi};
//+
Line Loop(3) = {3};
//+
Plane Surface(3) = {3};
//+
Line Loop(4) = {3};
//+
Plane Surface(4) = {4};
//+
Line Loop(5) = {3};
//+
Recursive Delete {
  Surface{2}; 
}
//+
BooleanDifference{ Surface{1}; Delete; }{ Surface{2}; Delete; }
//+
BooleanDifference{ Surface{1}; Delete; }{ Surface{2}; Delete; }
//+
Recursive Delete {
  Surface{2}; 
}
//+
Recursive Delete {
  Surface{2}; 
}
//+
Recursive Delete {
  Surface{2}; 
}
//+
Recursive Delete {
  Surface{2}; 
}
//+
Recursive Delete {
  Surface{2}; 
}
//+
Recursive Delete {
  Surface{2}; 
}
//+
Recursive Delete {
  Surface{2}; 
}
//+
Recursive Delete {
  Surface{2}; 
}
//+
Recursive Delete {
  Surface{2}; 
}
//+
Recursive Delete {
  Surface{2}; 
}
//+
Recursive Delete {
  Surface{2}; 
}
//+
Recursive Delete {
  Surface{2}; 
}
//+
Recursive Delete {
  Surface{2}; 
}
//+
Recursive Delete {
  Surface{2}; 
}
//+
Recursive Delete {
  Surface{2}; 
}
//+
Recursive Delete {
  Surface{2}; 
}
//+
Recursive Delete {
  Surface{2}; 
}
//+
Recursive Delete {
  Surface{2}; 
}
//+
Recursive Delete {
  Surface{2}; 
}
//+
Recursive Delete {
  Surface{2}; 
}
//+
Recursive Delete {
  Line{3}; 
}
//+
Recursive Delete {
  Line{3}; 
}
//+
SetFactory("Built-in");
//+
SetFactory("OpenCASCADE");
//+
SetFactory("OpenCASCADE");
//+
SetFactory("Built-in");
//+
SetFactory("Built-in");
//+
SetFactory("OpenCASCADE");
//+
SetFactory("OpenCASCADE");
//+
Recursive Delete {
  Surface{2}; Surface{1}; 
}
//+
Recursive Delete {
  Surface{2}; 
}
//+
Recursive Delete {
  Surface{2}; 
}
//+
Recursive Delete {
  Surface{2}; 
}
//+
Recursive Delete {
  Surface{2}; 
}
//+
Point(6) = {-1, 1, 0, 1.0};
//+
Recursive Delete {
  Point{6}; 
}
//+
Point(6) = {-1, 0, 1, 1.0};
//+
Point(7) = {1, 0, 1, 1.0};
//+
Line(4) = {3, 6};
//+
Line(5) = {6, 7};
//+
Line(6) = {7, 2};
//+
Line(7) = {2, 3};
//+
Line Loop(6) = {5, 6, 7, 4};
//+
Surface(5) = {6};
//+
BooleanDifference{ Surface{1}; Delete; }{ Surface{5}; Delete; }
//+
Recursive Delete {
  Surface{1}; 
}

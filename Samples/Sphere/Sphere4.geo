// Gmsh project created on Wed Dec 27 13:54:52 2017
SetFactory("OpenCASCADE");
//+
Sphere(1) = {0, 0, 0, 100, 0, Pi/2, 2*Pi};
//+
Sphere(2) = {0, 0, 0, 1000, 0, Pi/2, 2*Pi};
//+
Recursive Delete {
  Surface{4}; 
}
//+
Recursive Delete {
  Surface{4}; 
}
//+
Recursive Delete {
  Surface{4}; 
}
//+
Recursive Delete {
  Surface{4}; 
}
//+
Physical Surface("Radome") = {3};
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
  Surface{4}; 
}
//+
Recursive Delete {
  Surface{1}; 
}

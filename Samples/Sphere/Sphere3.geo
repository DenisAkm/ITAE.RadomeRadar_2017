// Gmsh project created on Wed Dec 27 13:42:37 2017
SetFactory("OpenCASCADE");
//+
Sphere(1) = {0, 0, 0, 10, 0, Pi/2, 2*Pi};
//+
Physical Surface("Radome") = {1};
//+
Physical Surface("Buttom") = {2};
//+
Physical Surface("Buttom") -= {2};
//+
Physical Surface("Buttom") -= {2};
//+
Physical Surface("Buttom") -= {2};
//+
Physical Surface("Buttom") -= {2};
//+
Physical Surface("Buttom") -= {2};
//+
Physical Surface("Buttom") -= {2};
//+
Physical Surface("Buttom") -= {2};
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
  Surface{2}; Surface{1}; 
}
//+
Recursive Delete {
  Surface{1}; 
}

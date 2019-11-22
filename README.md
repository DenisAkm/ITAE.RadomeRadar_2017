# RadomeRadar_Beta_2019
Программа для проведения электродинамических расчётов методом физической оптики системы "антенна-укрытие". 
Графический интерфейс и логика программы написаны на C# с помощью Windows Forms с использованием SlimDX для трёхмерного отображения математической модели 
и компоненты zedGraph для отображения результатов расчётов в виде графиков.
Решатель вынесен в отдельную библиотеку написанную на С++, поддерживающий распараллеливание через OpenMP.

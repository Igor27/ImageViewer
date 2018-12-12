using System;
using System.Windows;

namespace ImageViewer.Helpers
{
    public static class ActualSizeObserver
    {
        public const double DefaultHeight = 100.0;
        public const double DefaultWidth = 100.0;

        public static double IncreasedImageHeight { get; set; }
        public static double IncreasedImageWidth { get; set; }

        public static readonly DependencyProperty HeightProperty = DependencyProperty.RegisterAttached(
            "Height", typeof(double), typeof(ActualSizeObserver), new PropertyMetadata(DefaultHeight, OnHeightPropertyChanged));


        public static readonly DependencyProperty WidthProperty = DependencyProperty.RegisterAttached(
            "Width", typeof(double), typeof(ActualSizeObserver), new PropertyMetadata(DefaultWidth, OnWidthPropertyChanged));


        public static void SetHeight(DependencyObject element, double value)
        {
            element.SetValue(HeightProperty, value);
        }

        public static double GetHeight(DependencyObject element)
        {
            return (double)element.GetValue(HeightProperty);
        }

        public static void SetWidth(DependencyObject element, double value)
        {
            element.SetValue(WidthProperty, value);
        }

        public static double GetWidth(DependencyObject element)
        {
            return (double) element.GetValue(WidthProperty);
        }

        private static void OnHeightPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            IncreasedImageHeight = GetHeight(d);
        }

        private static void OnWidthPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            IncreasedImageWidth = GetWidth(d);
        }
    }
}
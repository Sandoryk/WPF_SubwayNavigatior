using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace SubwayNavigation
{
    class RouteBuilder:IAnimationOperator
    {
        Path ellipsePath;
        PointAnimationUsingPath centerPointAnimation;
        Storyboard pathAnimationStoryboard;
        Panel windowElement;
        bool allowOperations = false, routeIsInMotion = false;

        public RouteBuilder(Panel windowElement)
        {
            if (windowElement != null)
            {
                this.windowElement = windowElement;
                //Source: https://msdn.microsoft.com/en-us/library/ms746981(v=vs.110).aspx
                // Create a NameScope for the page so that
                // we can use Storyboards.
                NameScope.SetNameScope(windowElement, new NameScope());

                // Create the EllipseGeometry to animate.
                EllipseGeometry animatedEllipseGeometry =
                    new EllipseGeometry(new Point(10, 100), 10, 10);

                // Register the EllipseGeometry's name with
                // the page so that it can be targeted by a
                // storyboard.
                windowElement.RegisterName("AnimatedEllipseGeometry", animatedEllipseGeometry);

                // Create a Path element to display the geometry.
                ellipsePath = new Path();
                ellipsePath.Data = animatedEllipseGeometry;
                ellipsePath.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#f2ff00"));
                ellipsePath.Opacity = 0;
                ellipsePath.Stroke = Brushes.Black;
                ellipsePath.StrokeThickness = 3;
                ellipsePath.Margin = new Thickness(10);

                // Grid contains ellipsePath
                // and add it to the page.
                windowElement.Children.Add(ellipsePath);

                // Create a PointAnimationgUsingPath to move
                // the EllipseGeometry along the animation path.
                centerPointAnimation = new PointAnimationUsingPath();
                centerPointAnimation.Duration = TimeSpan.FromSeconds(3);
                centerPointAnimation.RepeatBehavior = RepeatBehavior.Forever;

                // Set the animation to target the Center property
                // of the EllipseGeometry named "AnimatedEllipseGeometry".
                Storyboard.SetTargetName(centerPointAnimation, "AnimatedEllipseGeometry");
                Storyboard.SetTargetProperty(centerPointAnimation,
                    new PropertyPath(EllipseGeometry.CenterProperty));

                // Create a Storyboard to contain and apply the animation.
                pathAnimationStoryboard = new Storyboard();
                //pathAnimationStoryboard.RepeatBehavior = RepeatBehavior.Forever;
                //pathAnimationStoryboard.AutoReverse = true;
                pathAnimationStoryboard.Children.Add(centerPointAnimation);
                allowOperations = true;
            }
        }
        public void BeginAnimation(Point[] path)
        {
            if (allowOperations && routeIsInMotion == false && path.Length>1)
            {
                // Create the animation path.
                PathGeometry animationPath = new PathGeometry();
                PathFigure pFigure = new PathFigure();
                pFigure.StartPoint = path[0];
                //pFigure.StartPoint = new Point(10, 100);
                PolyLineSegment plSegment = new PolyLineSegment();
                for (int i = 1; i < path.Length; i++)
                {
                    plSegment.Points.Add(path[i]);
                }

                pFigure.Segments.Add(plSegment);
                animationPath.Figures.Add(pFigure);

                // Freeze the PathGeometry for performance benefits.
                animationPath.Freeze();

                centerPointAnimation.PathGeometry = animationPath;
                centerPointAnimation.Duration = TimeSpan.FromSeconds(path.Length/2.7);
                //((PointAnimationUsingPath)pathAnimationStoryboard.Children.First()).PathGeometry = animationPath;

                ellipsePath.Opacity = 1;
                pathAnimationStoryboard.Begin(windowElement, true);
                routeIsInMotion = true;
            }
        }
        public void StopAnimation()
        {
            if (allowOperations && routeIsInMotion)
            {
                pathAnimationStoryboard.Stop(windowElement);
                ellipsePath.Opacity = 0;
                routeIsInMotion = false;
            }            
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SubwayNavigation
{
    public interface IAnimationOperator
    {
        void BeginAnimation(Point[] path);
        void StopAnimation();
    }
}

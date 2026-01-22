using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PhysicsLabsComplex
{
    internal class AnimatorButton
    {
        public static List<AnimationButton> AnimationButtonList = new List<AnimationButton>();

        public static int Count()
        {
            return AnimationButtonList.Count;
        }

        private static Thread AnimatorThread;

        private static double Interval;

        public static bool IsWork = false;

        public static void StartButton()
        {
            if (IsWork) return;

            IsWork = true;
            Interval = 14; // FPS ~66

            AnimatorThread = new Thread(AnimationInvoker)
            {
                IsBackground = true,
                Name = "UI Animation"
            };

            AnimatorThread.Start();
        }

        private static void AnimationInvoker()
        {
            while (IsWork)
            {
                AnimationButtonList.RemoveAll(a => a == null || a.Status == AnimationButton.AnimationStatus.Completed);

                Parallel.For(0, Count(), index =>
                {
                    AnimationButtonList[index].UpdateFrame();
                });

                Thread.Sleep((int)Interval);
            }
        }

        public static void Request(AnimationButton Anim, bool ReplaceIfExists = true)
        {
            if (AnimatorThread == null || IsWork == false)
            {
                StartButton();
            }

            Anim.Status = AnimationButton.AnimationStatus.Requested;

            AnimationButton dupAnim = GetDuplicate(Anim);

            if (dupAnim != null)
            {
                if (ReplaceIfExists == true)
                {
                    dupAnim.Status = AnimationButton.AnimationStatus.Completed;
                }
                else
                {
                    return;
                }
            }

            AnimationButtonList.Add(Anim);
        }

        private static AnimationButton GetDuplicate(AnimationButton Anim)
        {
            return AnimationButtonList.Find(a => a.ID == Anim.ID);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PhysicsLabsComplex
{
    public static class AnimatorToggle
    {
        public static List<AnimationToggle> AnimationList = new List<AnimationToggle>();

        public static int Count()
        {
            return AnimationList.Count;
        }

        private static Thread AnimatorThread;

        private static double Interval;

        public static bool IsWork = false;

        public static void StartToggle()
        {
            IsWork = true;
            Interval = 15; // FPS ~65

            AnimatorThread = new Thread(AnimationInvoker)
            {
                IsBackground = true,
                Name = "UI Animation"
            };

            AnimatorThread.Start();
        }

        private static void AnimationInvoker()
        {
            while (true)
            {
                AnimationList.RemoveAll(a => a.Status == AnimationToggle.AnimationStatus.Completed);

                Parallel.For(0, Count(), index =>
                {
                    AnimationList[index].UpdateFrame();
                });

                Thread.Sleep((int)Interval);
            }
        }

        public static void Request(AnimationToggle Anim, bool ReplaceIfExists)
        {
            if (AnimatorThread == null || IsWork == false)
            {
                StartToggle();
            }

            Anim.Status = AnimationToggle.AnimationStatus.Requested;

            AnimationToggle dupAnim = GetDuplicate(Anim);

            if (dupAnim != null)
            {
                if (ReplaceIfExists == true)
                {
                    dupAnim.Status = AnimationToggle.AnimationStatus.Completed;
                }
                else
                {
                    return;
                }
            }

            AnimationList.Add(Anim);
        }

        private static AnimationToggle GetDuplicate(AnimationToggle Anim)
        {
            return AnimationList.Find(a => a.ID == Anim.ID);
        }
    }
}

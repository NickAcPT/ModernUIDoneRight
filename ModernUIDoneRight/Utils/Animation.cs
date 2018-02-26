using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace NickAc.ModernUIDoneRight.Utils
{
    public class Animation
    {
        readonly Timer baseTimer;
        static List<String> globalCancel = new List<String>();
        Action<Animation> endAnim;
        bool hasBeenCanceled;
        string identifier = "unknown";

        public class AnimationBuilder
        {
            int count = 0;
            int countLimit = -1;
            int multiplier = 1;
            int interval = 1;
            String identifier = "unknown";
            Action<Animation> action;

            public AnimationBuilder WithAction(Action<Animation> a)
            {
                action = a;
                return this;
            }

            public AnimationBuilder WithCountLimit(int a)
            {
                var clone = (Action<Animation>)action.Clone();
                countLimit = a;
                action = new Action<Animation>((aa) => {
                    if (count < countLimit) {
                        clone.Invoke(aa);
                        count++;
                    } else
                        aa.Cancel();
                });
                return this;
            }

            public AnimationBuilder WithIdentifier(String a)
            {
                identifier = a;
                return this;
            }
            public AnimationBuilder WithInterval(int a)
            {
                interval = a;
                return this;
            }

            public AnimationBuilder WithMultiplier(int a)
            {
                multiplier = a;
                return this;
            }

            public Animation Build()
            {
                return new Animation(action, identifier, interval, multiplier);
            }
        }

        Animation(Action<Animation> action, String globalIdentifier = "unknown", int interval = 1, int multiplier = 1)
        {
            identifier = globalIdentifier;
            baseTimer = new Timer
            {
                Interval = interval
            };
            for (int i = 0; i < multiplier; i++) {
                baseTimer.Tick += (sender, e) => action?.Invoke(this);
            }
        }



        public Animation Start()
        {
            if (!globalCancel.Contains(identifier)) {
                baseTimer.Start();
                if (identifier != "unknown")
                    globalCancel.Add(identifier);
            }
            return this;
        }

        public Animation Cancel()
        {
            baseTimer.Stop();
            if (identifier != "unknown" && globalCancel.Contains(identifier))
                globalCancel.Remove(identifier);
            if (endAnim != null && (!hasBeenCanceled))
                endAnim(this);
            hasBeenCanceled = true;
            return this;
        }

        public Animation End(Action<Animation> end)
        {
            endAnim = end;
            return this;
        }
    }
}

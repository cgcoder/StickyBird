using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StickyBird.Objects;
using StickyBird.Screens;
using Juicy.Engine;
using Juicy.Engine.Juicy.Engine;

namespace StickyBird
{

    class Factory
    {
        public static Nest AddNest(PlayScreen screen, float x, float y)
        {
            Nest n = new Nest(World.CurrentWorld);
            n.IsGravityDisabled = true;
            AddObject(n, screen, x, y);

            return n;
        }

        public static LineObject AddLineObject(PlayScreen screen, float x, float y, float rotation, float angVelocity)
        {
            return AddLineObject(screen, "line", x, y, rotation, angVelocity);
        }

        public static LineObject AddHalfLineObject(PlayScreen screen, float x, float y, float rotation, float angVelocity)
        {
            return AddLineObject(screen, "lineh2", x, y, rotation, angVelocity);
        }

        public static LineObject AddLineObject(PlayScreen screen, string sprite, float x, float y, float rotation, float angVelocity)
        {
            LineObject line1 = new LineObject(World.CurrentWorld, sprite);
            AddObject(line1, screen, x, y);
            line1.Rotation = rotation;
            line1.AngularVelocity = angVelocity;

            return line1;
        }

        public static ElectricFence AddFence(PlayScreen screen, float x, float y, float rotation, float angVelocity)
        {
            ElectricFence fence = new ElectricFence(World.CurrentWorld);
            fence.Rotation = rotation;
            AddObject(fence, screen, x, y);

            return fence;
        }

        public static Spikes AddSpikes(PlayScreen screen, float x, float y)
        {
            Spikes s1 = new Spikes(World.CurrentWorld);
            s1.IsGravityDisabled = true;
            AddObject(s1, screen, x, y);

            return s1;
        }

        public static void AddObject(DynamicGameObject dgo, PlayScreen screen, float x, float y)
        {
            screen.ObjectManager.AddGameObject(dgo, PlayScreen.GAMEOBJ_BATCH);
            dgo.UpdatePosition(x, y);
            screen.AddCollidableObjects(dgo);
        }

        public static void AddSpikeRow(PlayScreen screen, int x, int y, int count)
        {
            int tx = x;
            for (int i = 0; i < count; i++)
            {
                Spikes s = AddSpikes(screen, tx, y);
                tx += s.W;
            }
        }

        public static StickyBirdObj AddBird(PlayScreen screen, int x, int y)
        {
            StickyBirdObj bird = new StickyBirdObj(World.CurrentWorld);
            //bird.IsGravityDisabled = true;
            screen.ObjectManager.AddGameObject(bird, PlayScreen.GAMEOBJ_BATCH);
            bird.UpdatePosition(x, y);

            return bird;
        }

        public static RedBall AddRedBall(PlayScreen screen, int x, int y, float angVel)
        {
            RedBall r = new RedBall(World.CurrentWorld);
            r.AngularVelocity = angVel;
            AddObject(r, screen, x, y);
            return r;
        }

        public static StarObject AddStar(PlayScreen screen, int x, int y)
        {
            StarObject so = new StarObject(World.CurrentWorld);
            AddObject(so, screen, x, y);
            return so;
        }

        public static void AddBackAndForthTransform(GameObj obj, int frameDuration, int deltaX, int deltaY)
        {
            ITransform t = new LinearTransform(0, frameDuration, deltaX, deltaY);
            t.AutoReset = true;
            ITransform t2 = new LinearTransform(frameDuration + 1, frameDuration + 1 + frameDuration, -deltaX, -deltaY);
            t2.AutoReset = true;

            obj.AddTransform(t);
            obj.AddTransform(t2);
        }
    }

    public class Level1Builder : ILevelBuilder
    {
        protected StarObject star1;
        protected StarObject star2;
        protected StarObject star3;
        protected PlayScreen screen;
        #region ILevelBuilder Members

        public virtual void BuildLevel(Screens.PlayScreen screen)
        {
            this.screen = screen;

            Factory.AddLineObject(screen, 150, 300, 0, 0);

            star1 = Factory.AddStar(screen, 225, 170);
            Factory.AddLineObject(screen, 400, 300, 0, 0);
            star2 = Factory.AddStar(screen, 400, 250);
            star3 = Factory.AddStar(screen, 540, 170);
            Factory.AddLineObject(screen, 650, 300, 0, 0);

            StickyBirdObj bird = Factory.AddBird(screen, 150, 250);
            screen.SetBird(bird);
            Factory.AddNest(screen, 650, 280);
            
            Factory.AddSpikeRow(screen, -100, GameConstants.ScreenHeight - 30, 10);
        }

        public virtual void resetGame(StickyBirdObj bird)
        {
            star1.Visible = true;
            star2.Visible = true;
            star3.Visible = true;

            screen.AddCollidableObjects(star1);
            screen.AddCollidableObjects(star2);
            screen.AddCollidableObjects(star3);

            bird.UpdatePosition(150, 250);
            bird.Stop();
            bird.IsAlive = true;
        }

        #endregion
    }

    public class Level2Builder : ILevelBuilder
    {
        private StarObject star1;
        private StarObject star2;
        private StarObject star3;
        private PlayScreen screen;

        #region ILevelBuilder Members

        public void BuildLevel(Screens.PlayScreen screen)
        {
            this.screen = screen;

            Factory.AddSpikeRow(screen, -100, GameConstants.ScreenHeight - 30, 10);
            Factory.AddLineObject(screen, 90, 400, 0, 0);
            star1 = Factory.AddStar(screen, 205, 230);
            Factory.AddFence(screen, 215, 380, (float) Math.PI/2, 0);
            star2 = Factory.AddStar(screen, 400, 260);
            LineObject l = Factory.AddLineObject(screen, 325, 400, 0, 0);
            Factory.AddBackAndForthTransform(l, 35, 175, 0);
            star3 = Factory.AddStar(screen, 575, 230);
            Factory.AddFence(screen, 585, 380, (float)Math.PI / 2, 0);
            l = Factory.AddLineObject(screen, 710, 400, 0, 0); // end
            StickyBirdObj bird = Factory.AddBird(screen, 120, 250);
            screen.SetBird(bird);
            Factory.AddNest(screen,700, 380);
        }

        public void resetGame(StickyBirdObj bird)
        {
            star1.Visible = true;
            star2.Visible = true;
            star3.Visible = true;

            screen.AddCollidableObjects(star1);
            screen.AddCollidableObjects(star2);
            screen.AddCollidableObjects(star3);

            bird.UpdatePosition(120, 250);
            bird.Stop();
            bird.IsAlive = true;
        }

        #endregion
    }

    public class Level3Builder : Level1Builder
    {
        public override void BuildLevel(Screens.PlayScreen screen)
        {
            this.screen = screen;

            Factory.AddSpikeRow(screen, -100, GameConstants.ScreenHeight - 30, 10);
            Factory.AddLineObject(screen, 90, 400, 0, 0);
            star1 = Factory.AddStar(screen, 205, 230);
            
            star2 = Factory.AddStar(screen, 400, 260);
            LineObject l = Factory.AddHalfLineObject(screen, 325, 400, 0, 0);
            Factory.AddBackAndForthTransform(l, 35, 175, 0);
            star3 = Factory.AddStar(screen, 575, 230);
            
            l = Factory.AddLineObject(screen, 710, 400, 0, 0); // end
            StickyBirdObj bird = Factory.AddBird(screen, 120, 250);
            screen.SetBird(bird);
            Factory.AddNest(screen, 700, 380);
        }

        public override void resetGame(StickyBirdObj bird)
        {
            star1.Visible = true;
            star2.Visible = true;
            star3.Visible = true;

            screen.AddCollidableObjects(star1);
            screen.AddCollidableObjects(star2);
            screen.AddCollidableObjects(star3);

            bird.UpdatePosition(150, 250);
            bird.Stop();
            bird.IsAlive = true;
        }
    }

    public class Level4Builder : ILevelBuilder
    {
        public void BuildLevel(PlayScreen screen)
        {
            Factory.AddSpikeRow(screen, -100, GameConstants.ScreenHeight - 30, 10);
            Factory.AddLineObject(screen, 90, 400, 0, 0);

            Factory.AddRedBall(screen, 250, 250, 0.03f);
            Factory.AddRedBall(screen, 500, 250, 0.03f);

            Factory.AddLineObject(screen, 710, 400, 0, 0); // end

            StickyBirdObj bird = Factory.AddBird(screen, 120, 250);
            screen.SetBird(bird);

            Factory.AddNest(screen, 700, 380);
        }

        public void resetGame(StickyBirdObj bird)
        {
            bird.UpdatePosition(150, 250);
            bird.Stop();
            bird.IsAlive = true;
        }
    }
    
}

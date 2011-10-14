using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Juicy.Engine;
using Microsoft.Xna.Framework.Content;
using StickyBird.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Graphics;

namespace StickyBird.Screens
{
    public class PlayScreen : JuicyScreen
    {
        public const int GAMEOBJ_BATCH = 2;

        private TextureCreator creator;
        private StickyBirdObj bird;

        private List<DynamicGameObject> collidableObjects;
        private ILevelBuilder builder;

        private int currentLevel;

        private Random random;

        /* Game state variables */
        private int stars;

        public PlayScreen()
        {
            this.backgroundResName = "background";
            random = new Random();
        }

        public override void LoadSprites(ContentManager conMan)
        {
            game.SprManager.LoadSprite("redbean");
            game.SprManager.LoadSprite("bird");
            game.SprManager.LoadSprite("bird2");
            game.SprManager.LoadSprite("line");
            game.SprManager.LoadSprite("lineh");
            game.SprManager.LoadSprite("stone");
            game.SprManager.LoadSprite("spikes");
            game.SprManager.LoadSprite("lighting");
            game.SprManager.LoadSprite("woodh");
            game.SprManager.LoadSprite("nest");
            game.SprManager.LoadSprite("stars");
            game.SprManager.LoadSprite("sparkle");
            game.SprManager.LoadSprite("rotator");

            // small paddle
            // end of small paddle
            
            game.SprManager.AddSprite("lineh2", Juicy.Engine.TextureCreator.CreateRectangleTexture(game.Graphics.GraphicsDevice, 50, 20,
                game.SprManager.GetSprite("line")));
            
            collidableObjects = new List<DynamicGameObject>();
        }

        public override void ScreenBecomesCurrent()
        {
            this.ObjectManager.ClearObjects(false);

            ResetLevel();
            currentLevel = 1;
            LoadNextLevel();
        }

        private void LoadNextLevel()
        {
            switch(currentLevel)
            {
                case 1:
                builder = new Level1Builder();
                break;
                case 2:
                builder = new Level2Builder();
                break;
                case 3:
                builder = new Level3Builder();
                break;
                case 4:
                builder = new Level4Builder();
                break;
            }

            builder.BuildLevel(this);
        }

        private void ResetLevel()
        {
            this.ObjectManager.ClearObjects(false);
            collidableObjects.Clear();
            stars = 0;
        }

        public void AddCollidableObjects(DynamicGameObject dgo)
        {
            if (collidableObjects.IndexOf(dgo) == -1)
            {
                collidableObjects.Add(dgo);
            }
        }

        public void RemoveCollidableObjects(DynamicGameObject dgo)
        {
            collidableObjects.Remove(dgo);
        }

        public void SetBird(StickyBirdObj bird)
        {
            this.bird = bird;
        }
        
        private void UpdateStickingData(IStickOn stickingOn)
        {
            if (bird.GetStickingOnObject() != stickingOn)
            {
                IStickOn on = bird.GetStickingOnObject();
                if (on != null)
                {
                    on.SetStickingObject(null);
                }
                bird.SetStickingOnObject(stickingOn);
                if (stickingOn != null)
                {
                    stickingOn.SetStickingObject(bird);
                    Vector2 requiredPos = (stickingOn as DynamicGameObject).RequiredCameraPosition;

                    this.game.UpdateTargetCameraPos(requiredPos.X, requiredPos.Y);
                }
            }
        }

        private void OnHitDeadlyObject()
        {
            bird.IsAlive = false;
            bird.IsGravityDisabled = true;
            bird.Stop();
            //builder.resetGame(bird);
        }

        private void OnHitBalance(DynamicGameObject dgo)
        {
            IStickOn stickOn = dgo as IStickOn;
            UpdateStickingData(stickOn);
            Vector2 np = dgo.CollisionDetector.FindExactCollidingPoint(bird.Shape);
            bird.UpdatePosition(np.X, np.Y);
        }

        private void OnHitStar(DynamicGameObject dgo)
        {
            stars++;
            if (dgo.Visible)
            {
                AddSparkle(dgo.Position);
            }
            dgo.Visible = false;
        }

        private void OnHitNest(DynamicGameObject dgo)
        {
            //this.AfterUpdateFuncDelegate = () =>
            //{
                ResetLevel();
                currentLevel++;
                if (currentLevel > 4) currentLevel = 1;
                LoadNextLevel();
            //};
        }

        public override bool HandleTouch(TouchCollection tc)
        {
            base.HandleTouch(tc);
            /*
             * TouchLocation tl = tc[0];
            bird.UpdatePosition(tl.Position.X, tl.Position.Y);
             * */
            return false;
        }

        private void AddSparkle(Vector2 pos)
        {
            for (int i = 0; i < 20; i++)
            {
                Sparkle s = new Sparkle(World.CurrentWorld);
                s.UpdatePosition(pos.X, pos.Y);
                s.ApplyImpulse(new Vector2((float)(2.5f - 5*random.NextDouble()), 
                        (float) (2.5f - 5*random.NextDouble())));
                s.Scale = 0.75f + (float) RandomUtil.Random.NextDouble()*0.25f;
                this.ObjectManager.AddGameObject(s, GAMEOBJ_BATCH);
            }
        }

        public override void HandleSwipe(Vector2 direction)
        {
            if (!bird.IsAlive)
            {
                bird.IsGravityDisabled = false;
                builder.resetGame(bird);
            }
            else
            {
                Vector2 td = new Vector2(direction.X / 30, direction.Y / 30);
                if (VectorUtil.Magnitude(td) > 8)
                {
                    Vector2 unit = VectorUtil.Unitize(td);
                    td.X = 8 * unit.X;
                    td.Y = 8 * unit.Y;
                }

                bird.ApplyImpulse(td);
                UpdateStickingData(null);
            }
        }

        public override void Update(GameTime time)
        {
            base.Update(time);

            if (bird.GetStickingOnObject() != null)
            {
                DynamicGameObject dgo = (bird.GetStickingOnObject() as DynamicGameObject);
                ICollisionDetector detector = dgo.CollisionDetector;

                (dgo as IStickOn).MoveStickingObject();
            }

            if (bird.IsAlive)
            {
                foreach (DynamicGameObject dgo in collidableObjects)
                {
                    if (bird.GetStickingOnObject() != dgo && dgo.CollisionDetector != null &&
                        dgo.CollisionDetector.DoesCollide(bird.Shape))
                    {
                        bird.AngularVelocity = 0f;
                        switch (dgo.Type)
                        {
                            case DynamicGameObject.ObjectType.Deadly:
                                OnHitDeadlyObject();
                                break;
                            case DynamicGameObject.ObjectType.Balance:
                                OnHitBalance(dgo);
                                break;
                            case DynamicGameObject.ObjectType.Star:
                                OnHitStar(dgo);
                                break;
                            case DynamicGameObject.ObjectType.Nest:
                                OnHitNest(dgo);
                                break;
                        }
                        break;// for loop
                    }
                }
            }
        }
    }

      
}

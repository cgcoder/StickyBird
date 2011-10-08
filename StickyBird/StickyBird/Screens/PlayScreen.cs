using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Juicy.Engine;
using Microsoft.Xna.Framework.Content;
using StickyBird.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;

namespace StickyBird.Screens
{
    public class PlayScreen : JuicyScreen
    {
        public const int GAMEOBJ_BATCH = 2;

        private TextureCreator creator;
        private StickyBirdObj bird;

        private List<DynamicGameObject> collidableObjects;
        private ILevelBuilder builder;

        /* Game state variables */
        private int stars;

        public PlayScreen()
        {
            this.backgroundResName = "background";
        }

        public override void LoadSprites(ContentManager conMan)
        {
            game.SprManager.LoadSprite("redbean");
            game.SprManager.LoadSprite("bird");
            game.SprManager.LoadSprite("line");
            game.SprManager.LoadSprite("stone");
            game.SprManager.LoadSprite("spikes");
            game.SprManager.LoadSprite("lighting");
            game.SprManager.LoadSprite("woodh");
            game.SprManager.LoadSprite("nest");
            game.SprManager.LoadSprite("stars");

            creator = new TextureCreator(game.SprManager, game.Graphics.GraphicsDevice);
            collidableObjects = new List<DynamicGameObject>();
        }

        public override void ScreenBecomesCurrent()
        {
            this.ObjectManager.ClearObjects(false);

            collidableObjects.Clear();

            builder = new Level1Builder();
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
            builder.resetGame(bird);
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
            dgo.Visible = false;
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

        public override void HandleSwipe(Vector2 direction)
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

        public override void Update(GameTime time)
        {
            base.Update(time);

            if (bird.GetStickingOnObject() != null)
            {
                DynamicGameObject dgo = (bird.GetStickingOnObject() as DynamicGameObject);
                ICollisionDetector detector = dgo.CollisionDetector;

                (dgo as IStickOn).MoveStickingObject();
            }

            foreach (DynamicGameObject dgo in collidableObjects)
            {
                if (bird.GetStickingOnObject() != dgo && dgo.CollisionDetector.DoesCollide(bird.Shape))
                {
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
                    }
                    break;// for loop
                }
            }
        }
    }

      
}

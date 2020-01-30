using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TileEngine
{
    public class NPC : Sprite
    {
        ConversationDialog dialog;
        Script script;

        public float ConversationRadius = 48;

        public NPC(Texture2D texture, Rectangle bounds, float radius, float speed, Script script, ConversationDialog dialog)
            : base(texture, bounds, radius, speed)
        {
            this.dialog = dialog;
            this.script = script;
        }

        public NPC(Animation anim, Rectangle bounds, float radius, float speed, Script script, ConversationDialog dialog)
            : base(anim, bounds, radius, speed)
        {
            this.dialog = dialog;
            this.script = script;
        }

        public NPC(Texture2D texture, Rectangle bounds, float radius, float speed, Script script, ConversationDialog dialog, float targetRadius)
            : base(texture, bounds, radius, speed, targetRadius)
        {
            this.dialog = dialog;
            this.script = script;
        }

        public NPC(Animation anim, Rectangle bounds, float radius, float speed, Script script, ConversationDialog dialog, float targetRadius)
            : base(anim, bounds, radius, speed, targetRadius)
        {
            this.dialog = dialog;
            this.script = script;
        }

        public override void Update(GameTime gt)
        {
            base.Update(gt);
        }

        public bool InConversationRange(Sprite s)
        {
            Vector2 d = Origin - s.Origin;
            return Engine.VectorLength(d) < ConversationRadius;
        }

        public void StartConversation(string name)
        {
            if (script == null || dialog == null)
                return;

            dialog.Npc = this;
            dialog.FirstFrame = true;
            dialog.Conversation = script[name];
            dialog.Open();
        }

        public void EndConversation()
        {
            if (script == null || dialog == null)
                return;

            dialog.Close();
        }

        public void StartFollowing()
        {
            FollowTarget = true;
        }

        public void StopFollowing()
        {
            FollowTarget = false;
        }

        public void LoseTarget()
        {
            Target = null;
        }
    }
}

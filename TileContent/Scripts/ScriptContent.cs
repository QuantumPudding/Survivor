using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Collections.ObjectModel;

namespace TileContent
{
    public class ScriptContent
    {
        public Collection<ConversationContent> Conversations = 
            new Collection<ConversationContent>();
    }

    public class ConversationContent
    {
        public Collection<ConversationHandlerContent> Handlers = 
            new Collection<ConversationHandlerContent>();
        public string Name;
        public string Text;
    }

    public class ConversationHandlerContent
    {
        public string Caption;
        public List<ConversationHandlerActionContent> Actions = 
            new List<ConversationHandlerActionContent>();
    }

    public class ConversationHandlerActionContent
    {
        public string MethodName;
        public object[] Parameters;
    }
}

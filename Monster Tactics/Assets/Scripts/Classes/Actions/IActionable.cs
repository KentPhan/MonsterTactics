using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Classes.Actions
{
    public interface IActionable
    {
        event EventHandler OnFinishedAction;
    }
}

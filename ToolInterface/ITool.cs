using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolInterface
{
    public interface ITool
    {
        string Name { get; }

        string Category { get; }

        string Description { get; }

        object Execute(object? input);

        string GetUI();

        void Stop();
    }


}

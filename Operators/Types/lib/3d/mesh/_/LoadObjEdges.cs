using System.Collections.Generic;
using System.Numerics;
using T3.Core.DataTypes;
using T3.Core.Logging;
using T3.Core.Operator;
using T3.Core.Operator.Attributes;
using T3.Core.Operator.Interfaces;
using T3.Core.Operator.Slots;
using T3.Core.Rendering;

namespace T3.Operators.Types.Id_dd3d7e16_f33e_4fb0_89c6_4d8cbc9d702f
{
    public class LoadObjEdges : Instance<LoadObjEdges>, IDescriptiveGraphNode
    {
        [Output(Guid = "C0D0420D-84E6-4C57-8E88-D2B04DB26B89")]
        public readonly Slot<StructuredList> Data = new Slot<StructuredList>();

        public LoadObjEdges()
        {
            Data.UpdateAction = Update;
        }

        private void Update(EvaluationContext context)
        {
            var path = Path.GetValue(context);
            if (path != _lastFilePath)
            {
                var mesh = ObjMesh.LoadFromFile(path);
                if (mesh == null)
                {
                    Log.Error($"Failed to extract edge line points from obj {path}", this);
                    return;
                }
                
                var hashSet = new HashSet<int>();

                foreach (var f in mesh.Faces)
                {
                    InsertVertexPair(f.V0, f.V1);
                    InsertVertexPair(f.V1, f.V2);
                    InsertVertexPair(f.V2, f.V0);
                }

                void InsertVertexPair(int from, int to)
                {
                    if (from < to)
                    {
                        (@from, to) = (to, @from);
                    }

                    var combined = (to << 16) + from;
                    hashSet.Add(combined);
                }

                var count = hashSet.Count;
                //_pointList = new T3.Core.DataStructures.Point[count * 3];
                _pointList.SetLength(count * 3);

                var index = 0;
                foreach (var pair in hashSet)
                {
                    var fromIndex = pair & 0xffff;
                    var toIndex = pair >> 16;

                    var pFrom = mesh.Positions[fromIndex];
                    _pointList.TypedElements[index] = new Point()
                                                          {
                                                              Position = new Vector3(pFrom.X, pFrom.Y, pFrom.Z),
                                                              Orientation = Quaternion.Identity,
                                                              W = 1
                                                          };
                    index++;

                    var pTo = mesh.Positions[toIndex];
                    _pointList.TypedElements[index] = new Point()
                                                          {
                                                              Position = new Vector3(pTo.X, pTo.Y, pTo.Z),
                                                              Orientation = Quaternion.Identity,
                                                              W = 1
                                                          };                    
                    index++;

                    _pointList.TypedElements[index] = Point.Separator();
                    index++;
                }

                _lastFilePath = path;
            }

            Data.Value = _pointList;
        }

        public InputSlot<string> GetSourcePathSlot()
        {
            return Path;
        }

        private readonly StructuredList<Point> _pointList = new StructuredList<Point>(10);

        private string _lastFilePath;

        [Input(Guid = "b6932cbd-e6b6-447b-b416-701326227864")]
        public readonly InputSlot<string> Path = new InputSlot<string>();
    }
}
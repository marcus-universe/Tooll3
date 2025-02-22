using System;
using T3.Core.DataTypes;
using T3.Core.Operator;
using T3.Core.Operator.Attributes;
using T3.Core.Operator.Slots;
using T3.Core.Resource;

namespace T3.Operators.Types.Id_f61ceb9b_74f8_4883_88ea_7e6c35b63bbd
{
    public class _BuildSpatialHashMap : Instance<_BuildSpatialHashMap>
    {
        [Output(Guid = "59d09aa6-051c-4906-9f32-f65e66979c56")]
        public readonly Slot<Command> Update = new Slot<Command>();
        
        [Output(Guid = "b4505f1e-ab0e-45be-8e46-8e3b37ec59ec")]
        public readonly Slot<SharpDX.Direct3D11.ShaderResourceView> CellPointIndices = new Slot<SharpDX.Direct3D11.ShaderResourceView>();

        [Output(Guid = "6c026a5f-a724-4240-bb96-077d65266f66")]
        public readonly Slot<SharpDX.Direct3D11.ShaderResourceView> PointCellIndices = new Slot<SharpDX.Direct3D11.ShaderResourceView>();

        [Output(Guid = "fb96e3d2-9a0f-466a-9b1d-997a4aa3e625")]
        public readonly Slot<SharpDX.Direct3D11.ShaderResourceView> HashGridCells = new Slot<SharpDX.Direct3D11.ShaderResourceView>();

        [Output(Guid = "13f0d2c2-a18b-457b-a3cf-cfd0c755b9a9")]
        public readonly Slot<SharpDX.Direct3D11.ShaderResourceView> CellPointCounts = new Slot<SharpDX.Direct3D11.ShaderResourceView>();

        [Output(Guid = "eeb282ee-ad73-471c-89ab-ae7cc8de6b15")]
        public readonly Slot<SharpDX.Direct3D11.ShaderResourceView> CellRangeIndices = new Slot<SharpDX.Direct3D11.ShaderResourceView>();



        [Input(Guid = "4bae9eaa-42d8-4c1c-8776-3abebcce20e2")]
        public readonly InputSlot<T3.Core.DataTypes.BufferWithViews> PointsA_ = new InputSlot<T3.Core.DataTypes.BufferWithViews>();

        [Input(Guid = "22f9737b-b3b4-4455-a4ec-8d61ab7abc6c")]
        public readonly InputSlot<float> CellSize = new InputSlot<float>();
    }
}


using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Physics;
using UnityEngine;
using UnityEngine.Rendering;

partial struct MoveSystem : ISystem
{

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {

        MoveUnitJobs moveUnitJobs = new MoveUnitJobs
        {
            deltaTime = SystemAPI.Time.DeltaTime,
        };

        moveUnitJobs.ScheduleParallel();

    }

}

[BurstCompile]
public partial struct MoveUnitJobs : IJobEntity
{
    public float deltaTime;
    private void Execute(ref LocalTransform localTransform, in MoveUnitComponent moveUnit, ref PhysicsVelocity physicsVelocity)
    {
        float3 moveDirection = moveUnit.TargetPosition - localTransform.Position;
        moveDirection = math.normalize(moveDirection);

        localTransform.Rotation = math.slerp(localTransform.Rotation,
            quaternion.LookRotation(moveDirection, math.up()),
            deltaTime * moveUnit.RotationSpeed);


        physicsVelocity.Linear = moveDirection * moveUnit.MoveSpeed;
        physicsVelocity.Angular = float3.zero;
    }
}

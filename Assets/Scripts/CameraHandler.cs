using UnityEngine;

public class CameraHandler : MonoBehaviour
{
   public Transform cameraTransform;
   public Transform pivotTransform;
   public Transform cameraHolderTransform;

   public CharacterStatus characterStatus;
   public CameraConfig cameraConfig;
   
   public bool leftPivot;
   public float deltaTime;

   public Transform targetLook;

   public float mouseX;
   public float mouseY;
   public float smoothX;
   public float smoothY;
   public float smoothXVelocity;
   public float smoothYVelocity;
   public float lookAngle;
   public float titleAngle;

   private void LateUpdate()
   {
      Tick();
   }

   void Tick()
   {
      deltaTime = Time.deltaTime;

      HandlePosition();
      HandleRotation();

      TargetLook();
      
      Debug.DrawLine(cameraTransform.position, targetLook.position, Color.red);
   }

   void TargetLook()
   {
      Ray ray = new Ray(cameraTransform.position, cameraTransform.forward * 2000);
      RaycastHit hit;

      if (Physics.Raycast(ray, out hit))
      {
         targetLook.position = Vector3.Lerp(targetLook.position, hit.point, deltaTime * 40);
         Vector3 direction = targetLook.position - cameraTransform.position;
         targetLook.rotation = Quaternion.LookRotation(direction);
      }
      else
      {
         targetLook.position = Vector3.Lerp(targetLook.position, targetLook.position + targetLook.transform.forward * 200, deltaTime * 5);
         Vector3 direction = targetLook.position - cameraTransform.position;
         targetLook.rotation = Quaternion.LookRotation(direction);
      }
   }

   void HandlePosition()
   {
      float targetX = cameraConfig.normalX;
      float targetY = cameraConfig.normalY;
      float targetZ = cameraConfig.normalZ;

      if (characterStatus.isAiming)
      {
         targetX = cameraConfig.aimX;
         targetZ = cameraConfig.aimZ;
      }

      if (leftPivot)
      {
         targetX = -targetX; 
      }

      Vector3 newPivotPosition = pivotTransform.localPosition;
      newPivotPosition.x = targetX;
      newPivotPosition.y = targetY;

      Vector3 newCameraPosition = cameraTransform.localPosition;
      newCameraPosition.z = targetZ;

      float time = deltaTime * cameraConfig.pivotSpeed;
      pivotTransform.localPosition = Vector3.Lerp(pivotTransform.localPosition, newPivotPosition, time);
      cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, newCameraPosition, time);
      
   }

   void HandleRotation()
   {
      mouseX = Input.GetAxis("Mouse X");
      mouseY = Input.GetAxis("Mouse Y");

      if (cameraConfig.turnSmooth > 0)
      {
         smoothX = Mathf.SmoothDamp(smoothX, mouseX, ref smoothXVelocity, cameraConfig.turnSmooth);
         smoothY = Mathf.SmoothDamp(smoothY, mouseY, ref smoothYVelocity, cameraConfig.turnSmooth);
      }
      else
      {
         smoothX = mouseX;
         smoothY = mouseY;
      }

      lookAngle += smoothX * cameraConfig.Y_rot_speed;
      Quaternion targetRotation = Quaternion.Euler(0, lookAngle, 0);
      cameraHolderTransform.rotation = targetRotation;

      titleAngle -= smoothY * cameraConfig.X_rot_speed;
      titleAngle = Mathf.Clamp(titleAngle, cameraConfig.minAngle, cameraConfig.maxAngle);
      
      pivotTransform.localRotation = Quaternion.Euler(titleAngle, 0, 0);
   }
}
using UnityEngine;

public class CameraControll : MonoBehaviour
{
    private Transform cameraPosition;

    private float horizontalInput;
    private float verticalInput;

    private float speed = 1f;

    private TileMap tileMap;

    private int xLengthBorder;
    private int zLengthBorder;

    private int bordersOffsets = 1;

    private void Start()
    {
        cameraPosition = this.gameObject.GetComponent<Transform>();
        tileMap = GameObject.FindGameObjectWithTag("Map").GetComponent<TileMap>();
        xLengthBorder = tileMap.map.GetLength(0) + bordersOffsets;
        zLengthBorder = tileMap.map.GetLength(1) - bordersOffsets;
    }
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(horizontalInput * speed * Time.deltaTime, 0, 0);

        if(cameraPosition.position.x + move.x > bordersOffsets && cameraPosition.position.x + move.x < xLengthBorder)
            cameraPosition.position += move;

        move = new Vector3(0, 0, verticalInput * speed * Time.deltaTime);

        if (cameraPosition.position.z + move.z > -bordersOffsets && cameraPosition.position.z + move.z < zLengthBorder)
            cameraPosition.position += move;
    }
}

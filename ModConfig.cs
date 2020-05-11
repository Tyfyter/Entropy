/*[MainConfig]
public class ExampleConfig : ModConfig {
    public Clientside clientside;
    public Gameplay gameplay;
}

public class Clientside : ModConfig{
    public BloodAndGore bloodAndGore;
    public CameraSettings cameraSettings;
    public bool allowSwears;
}
public class BloodAndGore : ModConfig{
    public bool enableBlood;
    public bool enableBloodOnTiles;
    public int maxGores;
    public bool goreStay;
}
public class CameraSettings : ModConfig{
    public bool fixedCamera;
    public bool smoothCamera;
}

public class Gameplay : ModConfig{
    public bool enableHolidays;
    public bool enableJumping;
}

/* JSON:{
    "clientside": {
        "bloodAndGore": {
            "enableBlood": true,
            "enableBloodOnTiles": true,
            "maxGores": 400,
            "goreStay": false
        },
        "cameraSettings": {
            "fixedCamera": true,
            "smoothCamera": true
        },
        "allowSwears": false
    },
    "gameplay": {
        "enableHolidays": true,
        "enableJumping": false
    }
}
*/
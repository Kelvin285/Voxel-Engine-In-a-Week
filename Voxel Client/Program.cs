using VoxelEngineClient;

[System.Runtime.InteropServices.DllImport("nvapi64.dll", EntryPoint = "fake")]
static extern int LoadNvApi64();

[System.Runtime.InteropServices.DllImport("nvapi.dll", EntryPoint = "fake")]
static extern int LoadNvApi32();

try
{
    if (Environment.Is64BitProcess)
        LoadNvApi64();
    else
        LoadNvApi32();
}
catch { }

new GameClient();
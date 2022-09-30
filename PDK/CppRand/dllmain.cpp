#include "windows.h"
#include <random>

extern "C" {
    __declspec(dllexport) void SeedDll(int value) {
        std::srand(value);
    }
    __declspec(dllexport) int GetRandIntDll() {
        return std::rand();
    }
    __declspec(dllexport) int GetRandIntDllRanged(int startRange, int endRange) {
        return startRange + (std::rand() % static_cast<int>(endRange - startRange + 1));
    }
}

BOOL APIENTRY DllMain(HMODULE hModule, DWORD  ul_reason_for_call, LPVOID lpReserved) {
    return TRUE;
}


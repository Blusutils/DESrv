// dllmain.cpp : Определяет точку входа для приложения DLL.
#include "windows.h"
#include <random>


_declspec(dllexport) void SeedDll(int value) {
    std::srand(value);
}
_declspec(dllexport) int GetRandIntDll() {
    return std::rand();
}
_declspec(dllexport) int GetRandIntDll(int startRange, int endRange) {
    return startRange + (std::rand() % static_cast<int>(endRange - startRange + 1));
}

BOOL APIENTRY DllMain(HMODULE hModule, DWORD  ul_reason_for_call, LPVOID lpReserved) {
    return TRUE;
}


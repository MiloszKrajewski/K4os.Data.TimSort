@echo off
pushd %~dp0
dotnet run --project ./src/Benchmarks/ --configuration Release -- %*
popd
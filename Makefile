all:
	dotnet build RealityV
	
test: all
	cp "RealityV/bin/Debug/netstandard2.0/RealityV.dll" "/home/cam/.local/share/Steam/steamapps/common/Grand Theft Auto V/scripts/"
	
clean:
	rm -rf RealityV/bin

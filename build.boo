solution_file = "src/HuanlinLib.sln"
assembly_info_file = "src/AssemblyInfo_Global.cs"
configuration = "Release"
properties_x86 = {"Platform" : "x86"}
properties_x64 = {"Platform" : "x64"}

desc "預設的 target"
target default, (init, build, deploy):
  pass

desc "建置前的初始化動作"
target init:
  rmdir("build")

desc "清除編譯過程產生的檔案（讓原始碼保持乾淨）"  
target clean:
  rmdir("build")
  rmdir("src/Huanlin/bin")
  rmdir("src/Huanlin/obj")   
  rmdir("src/Huanlin.AppBlock/bin")
  rmdir("src/Huanlin.AppBlock/obj")   
  rmdir("src/Huanlin.Braille/bin")
  rmdir("src/Huanlin.Braille/obj")   
  rmdir("src/Huanlin.TextServices/bin")
  rmdir("src/Huanlin.TextServices/obj")   
  rmdir("src/Huanlin.WinApi/bin")
  rmdir("src/Huanlin.WinApi/obj")   
  rmdir("src/Huanlin.WinForms/bin")
  rmdir("src/Huanlin.WinForms/obj")   
  rmdir("src/tests/Test.Huanlin/bin")
  rmdir("src/tests/Test.Huanlin/obj")
  rmdir("src/tests/Test.Huanlin.Braille/bin")
  rmdir("src/tests/Test.Huanlin.Braille/obj")
  rmdir("src/tests/Test.Huanlin.TextServices/bin")
  rmdir("src/tests/Test.Huanlin.TextServices/obj")  

desc "建置專案"
target build:
  print "產生組件版本編號..."
  exec("tools/UpdateVersion.exe", "-b Increment -r yyyy -i ${assembly_info_file} -o ${assembly_info_file}")
  
  print "建置專案..."

desc "將建置輸出結果複製到 'build' 資料夾"
target deploy:
  print "複製檔案到 build 資料夾..."
    
  with FileList(): 
    .Include("readme.txt")
    .ForEach def(file):
      file.CopyToDirectory("build/x86/${configuration}")
      file.CopyToDirectory("build/x64/${configuration}")

  with FileList("src/Huanlin/bin/x86/${configuration}"):
    .Include("*.{dll,exe}")
    .ForEach def(file):
      file.CopyToDirectory("build/x86/${configuration}/bin")

  with FileList("src/Huanlin/bin/x64/${configuration}"):
    .Include("*.{dll,exe}")
    .ForEach def(file):
      file.CopyToDirectory("build/x64/${configuration}/bin")

  with FileList("src/Huanlin.AppBlock/bin/x86/${configuration}"):
    .Include("*.{dll,exe}")
    .ForEach def(file):
      file.CopyToDirectory("build/x86/${configuration}/bin")

  with FileList("src/Huanlin.AppBlock/bin/x64/${configuration}"):
    .Include("*.{dll,exe}")
    .ForEach def(file):
      file.CopyToDirectory("build/x64/${configuration}/bin")

  with FileList("src/Huanlin.Braille/bin/x86/${configuration}"):
    .Include("*.{dll,exe}")
    .Include("Braille.ini")
    .ForEach def(file):
      file.CopyToDirectory("build/x86/${configuration}/bin")

  with FileList("src/Huanlin.Braille/bin/x64/${configuration}"):
    .Include("*.{dll,exe}")
    .Include("Braille.ini")
    .ForEach def(file):
      file.CopyToDirectory("build/x64/${configuration}/bin")


  with FileList("src/Huanlin.TextServices/bin/x86/${configuration}"):
    .Include("*.{dll,exe}")
    .ForEach def(file):
      file.CopyToDirectory("build/x86/${configuration}/bin")

  with FileList("src/Huanlin.TextServices/bin/x64/${configuration}"):
    .Include("*.{dll,exe}")
    .ForEach def(file):
      file.CopyToDirectory("build/x64/${configuration}/bin")

  with FileList("src/Huanlin.WinApi/bin/x86/${configuration}"):
    .Include("*.{dll,exe}")
    .ForEach def(file):
      file.CopyToDirectory("build/x86/${configuration}/bin")
  
  with FileList("src/Huanlin.WinApi/bin/x64/${configuration}"):
    .Include("*.{dll,exe}")
    .ForEach def(file):
      file.CopyToDirectory("build/x64/${configuration}/bin")

  with FileList("src/Huanlin.WinForms/bin/x86/${configuration}"):
    .Include("*.{dll,exe}")
    .ForEach def(file):
      file.CopyToDirectory("build/x86/${configuration}/bin")

  with FileList("src/Huanlin.WinForms/bin/x64/${configuration}"):
    .Include("*.{dll,exe}")
    .ForEach def(file):
      file.CopyToDirectory("build/x64/${configuration}/bin")

  with FileList("lib/ImeLib/x86"):
    .Include("*.dll")
    .ForEach def(file):
      file.CopyToDirectory("build/x86/${configuration}/bin")

  with FileList("lib/ImeLib/x64"):
    .Include("*.dll")
    .ForEach def(file):
      file.CopyToDirectory("build/x64/${configuration}/bin")

desc "建立 zip 壓縮檔"
target package:
  zip("build/${configuration}", 'build/HuanlinLib.zip')


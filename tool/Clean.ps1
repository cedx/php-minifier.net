"Deleting all generated files..."
Remove-Item bin -ErrorAction Ignore -Force -Recurse
Remove-Item src/*/obj -Force -Recurse
Remove-Item test/obj -ErrorAction Ignore -Force -Recurse
Remove-Item var/* -Exclude .gitkeep -Force -Recurse

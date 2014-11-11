import os
f = open("field.txt", "w")
for i in range(10):
    for j in range(10):
        f.write('<Grid Grid.Row="' + str(i + 1) + '" Grid.Column="' + str(j + 1) + '">\n')
        f.write('<Rectangle Name="opcell' + str(i*10+j) + '" Style="{StaticResource EmptyCell}" MouseLeftButtonUp="Cell_LeftClick"/>\n')
        f.write('</Grid>\n')
f.close()

from firebase import firebase
import random
import pandas as pd
import matplotlib.pyplot as plt

firebase = firebase.FirebaseApplication('https://experimentsdb.firebaseio.com',None)


random.seed(13)
def equation(x, a):
    y = 3 * x + a * 2 + random.randint(-1,1)
    return y


data = {}
data['xa'],data['ax'] = [],[]
data['output'] = []



for x in range(0,150):
    data['xa'].append(x)
    a = random.randint(-8,8)
    data['ax'].append(a)
    data['output'].append(equation(x, a))

df = pd.DataFrame(data, columns=['xa','ax', 'output'])
df.to_csv('eval.csv', index=False)

firebase.post("Linear/", data)

print(df)

df.cumsum()
plt.figure()
df.plot()
plt.show()
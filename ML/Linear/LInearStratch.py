import matplotlib.pyplot as plt
import matplotlib.animation as animation
import numpy as np
import pandas as pd

from matplotlib.animation import FFMpegFileWriter



trainPD = pd.read_csv('train.csv')
evalPD = pd.read_csv('eval.csv')

def featureScaling(X): #deal with numpy array
    for i, x in enumerate(X):
        X[i] = ((x - np.average(X))/np.ptp(X))
    return X

def hypo(theta, x):
    return np.matmul(np.transpose(x), theta)

def pdCost(X, y, theta, j, learningRate = 8.5):


    total = 0
    for i in range(0, len(X)):
        #print(f'x: {X[i]}, y: {y[i]}, theta: {theta}')
        total +=((hypo(theta, X[i]) - y[i]) * X[i][j])
    return (learningRate * (total / (len(X))))


def Cost(X, y):
    total = np.sum(X-y)
    return total / len(X)



def xZero(X):
    allOne = np.ones(shape=(len(X.index),))
    X.insert(loc=0, column = 'zeros', value=allOne)
    return (X)

def linearRegressor(trainDatabase):
    repeat = 300
    y = np.array(trainDatabase.pop('output'))



    trainDatabase = xZero(trainDatabase)
    X = featureScaling(np.array(trainDatabase))

    initialTheta = np.zeros(shape = (len(trainDatabase.columns),))
    #find theta cost
    costs = []

    print(Cost([hypo(initialTheta,X[i]) for i in range(len(X))], y))

    theta = []

    untilIndex = 100



    for i in range(repeat):
        tempoTheta = initialTheta
        for j in range(len(trainDatabase.columns)):
            cost = pdCost(X, y, initialTheta, j)
            tempoTheta[j] -= cost
            if j == 0:
                costs.append(cost)

        initialTheta = tempoTheta
        plt.figure()
        plt.ylim(0,330)
        data = pd.DataFrame({'Prediction':[hypo(tempoTheta, X[b]) for b in range(untilIndex)], 'Result':y[:untilIndex]})

        plt.plot(data['Prediction'], label='Prediction')
        plt.plot(data['Result'], label='Result')
        plt.legend()
        plt.title(f'Generation: {i}')
        plt.savefig(f'!frame{i}.png')
        print(f'No.{i} frame rendered')
        plt.close()
        theta.append(initialTheta)










linearRegressor(trainPD)

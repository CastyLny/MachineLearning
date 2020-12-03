import matplotlib.pyplot as plt
import matplotlib.animation as animation
import numpy as np
import pandas as pd

from matplotlib.animation import FFMpegFileWriter

'''
Material 1 amount
Material 2 amount
Material 3 amount
Condition
Octane number'''

trainPD = pd.read_csv('OctaneLevel.csv')
evalPD = pd.read_csv('eval.csv')

def featureScaling(X): #deal with numpy array
    for i, x in enumerate(X):
        X[i] = ((x - np.average(X))/np.ptp(X))
    return X

def hypo(theta, x):
    return np.matmul(np.transpose(x), theta)

def pdCost(X, y, theta, j, learningRate = 2):

    total = 0
    for i in range(0, len(X)):


        total +=((hypo(theta, X[i]) - y[i]) * X[i][j])
    return (learningRate * (total / (len(X))))


def Cost(X, y):
    total = np.abs(np.sum(X-y))
    return total / len(X)



def xZero(X):
    allOne = np.ones(shape=(X.shape[0], X.shape[1] + 1))
    allOne[:,1:] = X
    return allOne

def linearRegressor(trainDatabase, predictionFeature, popFeature, Visualize = False):
    repeat = 1000
    y = np.array(trainDatabase.pop(predictionFeature))
    for feature in popFeature:
        trainDatabase.pop(feature)
    X = featureScaling(np.array(trainDatabase))
    X = xZero(X)
    initialTheta = np.ones(shape = (X.shape[1],))
    #find theta cost
    costs = []

    #print(Cost([hypo(initialTheta,X[i]) for i in range(len(X))], y))

    theta = []

    untilIndex = 61

    print(X)

    for i in range(repeat):
        tempoTheta = initialTheta
        cost = 0

        for j in range(X.shape[1]):
            cost = pdCost(X, y, initialTheta, j)
            tempoTheta[j] -= cost
            if j == 0:
                costs.append(cost)

        initialTheta = tempoTheta
        if Visualize:
            plt.figure()
            plt.ylim(0, np.max(y) + 30)
            data = pd.DataFrame(
                {'Prediction': [hypo(tempoTheta, X[b]) for b in range(untilIndex)], 'Result': y[:untilIndex]})
            plt.scatter(range(untilIndex), data['Result'], label='Result')
            plt.plot(data['Prediction'], 'r', label='Prediction')

            plt.legend()
            plt.title(f'Generation: {i}, Cost: {str(cost * 10000)}')
            plt.savefig(f'!frame{i}.png')
            print(f'No.{i} frame rendered')
            plt.close()
            theta.append(initialTheta)

    print(np.floor(costs[0]))



linearRegressor(trainPD, 'Octane', ['Mat1Amt','Condition'])

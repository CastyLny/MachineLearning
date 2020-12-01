import matplotlib.pyplot as plt
import numpy as np
import pandas as pd

trainPD = pd.read_csv('train.csv')
evalPD = pd.read_csv('eval.csv')

def featureScaling(X):
    for i, x in enumerate(X):
        X[i] = ((x - np.average(X))/np.ptp(X))
    return X

def hypo(theta, x):
    return np.matmul(np.transpose(x), theta)

def pdCost(X, y, theta, j, learningRate = 1):


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
    repeat = 10000
    y = np.array(trainDatabase.pop('output'))



    trainDatabase = xZero(trainDatabase)
    X = featureScaling(np.array(trainDatabase))
    print('poo: ' + str(np.ptp(X)))

    initialTheta = np.ones(shape = (len(trainDatabase.columns),))
    #find theta cost
    costs = []

    print(Cost([hypo(initialTheta,X[i]) for i in range(len(X))], y))



    for i in range(repeat):
        tempoTheta = initialTheta
        for j in range(len(trainDatabase.columns)):
            cost = pdCost(X, y, initialTheta, j)
            tempoTheta[j] -= cost
            if j == 0:
                costs.append(cost)

        initialTheta = tempoTheta

    untilIndex = 100
    Eval = pd.DataFrame({'Results': [hypo(initialTheta,X[i]) for i in range(untilIndex)], 'Evaluate':y[0:untilIndex]})

    print(Cost([hypo(initialTheta,X[i]) for i in range(untilIndex)], y[0:untilIndex]))


    print(Eval)


    plt.figure()
    plt.plot(Eval)
    plt.show()












linearRegressor(trainPD)

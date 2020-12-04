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

trainPD = pd.read_csv('train.csv')
evalPD = pd.read_csv('eval.csv')

def featureScaling(X, average, Range): #deal with numpy array

    X = ((X - average)/Range)
    print('X'+ str(X))
    return X

def hypo(theta, x):

    result = np.matmul(np.transpose(x), theta)
    return result

def pdCost(X, y, theta, j, learningRate = 0.5):
    total = 0
    for i in range(0, len(X)):
        total +=((hypo(theta, X[i]) - y[i]) * X[i][j]) * learningRate
        #print(total)
    return (total/list(X.shape)[0])


def Cost(X, y):
    total = np.abs(np.sum(X-y))
    return total / len(X)



def xZero(X):
    #print(X)
    allOne = np.ones(shape=(X.shape[0], X.shape[1] + 1))
    allOne[:,1:] = X
    #print(allOne)
    return allOne

def linearRegressor(trainDatabase, predictionFeature, measure, popFeature = [], Visualize = False):
    repeat = 1100
    y = np.array(trainDatabase.pop(predictionFeature))
    for feature in popFeature:
        trainDatabase.pop(feature)

    X = np.array(trainDatabase)
    aver = np.average(X, axis=0)
    ptp = np.max(X, axis=0) - np.min(X, axis=0)
    X = xZero(featureScaling(X, aver, ptp))

    initialTheta = np.zeros(shape = (X.shape[1],))
    #find theta cost
    costs = []

    print(initialTheta)

    #print(Cost([hypo(initialTheta,X[i]) for i in range(len(X))], y))

    theta = []

    untilIndex = 61


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



    for feature in popFeature:
            measure.pop(feature)
    yEval = np.array(measure.pop(predictionFeature))
    xEval = np.array(measure)
    xEval = featureScaling(xEval, aver, ptp)
    xEval = xZero(xEval)

    print(f"Theta: {tempoTheta}")

    PredictedLine = [hypo(tempoTheta, xEval[b]) for b in range(list(xEval.shape)[0])]

    Accuracy = Cost(PredictedLine,yEval)
    plt.figure()
    plt.scatter(range(len(yEval)), yEval, label="Evaluated data")
    plt.plot(PredictedLine, 'r', label = "Evaluated line")
    plt.legend()
    if Visualize:
        for i in range(repeat, repeat + 50):
            plt.savefig(f'!frame{i}.png')
    plt.show()


    print(Accuracy)





linearRegressor(trainPD, 'output', evalPD, Visualize=True)
